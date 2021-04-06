using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Models;
using Nedelko.Polyclinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly PolyclinicContext _polyclinicContext;
        private readonly UserManager<User> _userManager;

        public PatientController(
            PolyclinicContext polyclinicContext,
            UserManager<User> userManager)
        {
            _polyclinicContext = polyclinicContext ?? throw new ArgumentNullException(nameof(polyclinicContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> Detailes(Guid id)
        {
            var patient = await _polyclinicContext.Patients.FindAsync(id);

            var patientViewModel = new PatientViewModel
            {
                FName = patient.FName,
                LName = patient.LName,
                Age = patient.Age,
                BDate = patient.BDate,
                EnumStatus = patient.Status,
                AreaId = patient.AreaId,
            };

            ViewBag.Address = await _polyclinicContext.PatientAddresses.FindAsync(id);

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Email = (await _userManager.FindByIdAsync(id.ToString())).Email;

            return View(patientViewModel);
        }

        public async Task<IActionResult> Show()
        {
            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            return View(await _polyclinicContext.Patients.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> FindAsync(string patientToFind)
        {
            var patient =
                await _polyclinicContext.Patients
                    .AsNoTracking()
                    .ToListAsync();
            
            var patients = new List<Patient>();
            foreach (var p in patient)
            {
                if (p.LName == patientToFind)
                {
                    patients.Add(p);
                }
            }

            ViewBag.PatFind = patients;
            ViewBag.Doctor = User.Identity.Name;

            return View("/Views/Visit/CreateVisitDoctor.cshtml");
        }

        public async Task<IActionResult> FindPatientModel(string patientToFind)
        {
            var pat = _polyclinicContext.Patients.ToList();
            List<PatientViewModel> patients = new List<PatientViewModel>();
            foreach (var p in pat)
            {
                if (p.LName == patientToFind)
                {
                    PatientViewModel model = new PatientViewModel
                    {
                        Id = p.Id,
                        FName = p.FName,
                        LName = p.LName,
                        Age = p.Age,
                        BDate = p.BDate,
                        EnumStatus = p.Status,
                        AreaId = p.AreaId,
                    };
                    patients.Add(model);
                }
            }
            ViewBag.PatFind = patients;
            ViewBag.Areas = await _polyclinicContext.Areas.AsNoTracking().ToListAsync();
            return View("~/Views/Doctor/ShowMyPatients.cshtml", patients);
        }

        public async Task<IActionResult> ShowMyVisits(Guid id)
        {
            var visits =
                await _polyclinicContext.Visits
                    .AsNoTracking()
                    .Where(v => v.PatientId == id)
                    .ToListAsync();

            if (!visits.Any())
            {
                ViewBag.Message = "No Visits were found.";
                return View();
            }

            var doctors = new List<Doctor>();
            var patient = await _polyclinicContext.Patients.FindAsync(id);
            ViewBag.PatientName = string.Concat(patient.FName, " ", patient.LName);
            
            foreach (var v in visits)
            {
                doctors.Add(await _polyclinicContext.Doctors.FindAsync(v.DoctorId));
            }

            ViewBag.Doctors = doctors;

            return View(visits);
        }

        public async Task<IActionResult> ShowMyPrescriptions(Guid id)
        {
            var prescriptions =
                await _polyclinicContext.Prescriptions
                    .AsNoTracking()
                    .Where(pr => pr.PatientId == id)
                    .ToListAsync();

            var patient = await _polyclinicContext.Patients.FindAsync(id);

            if (!prescriptions.Any())
            {
                ViewBag.Patient = string.Concat(patient.FName, " ", patient.LName);
                ViewBag.Message = "NO Prescriptions found.";
                return View();
            }

            ViewBag.Doctors = from pr in prescriptions
                              from doctor in _polyclinicContext.Doctors
                              where pr.DoctorId == doctor.Id
                              select doctor;

            ViewBag.Patient = string.Concat(patient.FName, " ", patient.LName);

            return View(prescriptions);
        }

        public async Task<IActionResult> ShowMyDoctors(Guid id)
        {
            var visits =
                await _polyclinicContext.Visits
                    .AsNoTracking()
                    .Where(v => v.PatientId == id)
                    .ToListAsync();

            var patient = await _polyclinicContext.Patients.FindAsync(id);

            if (!visits.Any())
            {
                ViewBag.Patient = string.Concat(patient.FName, " ", patient.LName);
                ViewBag.Message = "No Doctors found.";
                return View();
            }

            ViewBag.Patient = string.Concat(patient.FName, " ", patient.LName);

            var doctorIds = (from d in visits select d.DoctorId).ToHashSet();
            var doctors = new List<Doctor>();
            foreach (var docId in doctorIds)
            {
                doctors.Add(await _polyclinicContext.Doctors.FindAsync(docId));
            }

            return View(doctors);
        }
    }
}
