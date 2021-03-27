using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Interfaces;
using Nedelko.Polyclinic.Managers;
using Nedelko.Polyclinic.Models;
using Nedelko.Polyclinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    public class VisitController : Controller
    {
        private readonly PolyclinicContext _polyclinicContext;
        private readonly ISmsManager _smsManager;

        public VisitController(
            PolyclinicContext polyclinicContext,
            ISmsManager smsManager)
        {
            _polyclinicContext = polyclinicContext ?? throw new ArgumentNullException(nameof(polyclinicContext));
            _smsManager = smsManager ?? throw new ArgumentNullException(nameof(smsManager));
        }

        public async Task<IActionResult> Info(Guid id)
        {
            var visit = await _polyclinicContext.Visits.FindAsync(id);
            var visitViewModel = new VisitViewModel
            {
                Description = visit.Description,
                Date = visit.Date,
                Diagnosis = visit.Diagnosis,
                PatientId = visit.PatientId,
            };

            ViewBag.Patient = await _polyclinicContext.Patients.FindAsync(visit.PatientId);
            ViewBag.Doctor = await _polyclinicContext.Doctors.FindAsync(visit.DoctorId);
            ViewBag.Prescriptions = 
                await _polyclinicContext.Prescriptions
                    .AsNoTracking()
                    .Where(presc => presc.VisitId == id)
                    .ToListAsync();

            return View(visitViewModel);
        }

        public async Task<IActionResult> CreateReservationPatient(EventViewModel model, string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var events = 
                await _polyclinicContext.Events
                    .AsNoTracking()
                    .Where(e => e.DoctorId == model.DoctorId)
                    .ToListAsync();

            model.End = model.Start.AddMinutes(30);
            foreach (var e in events)
            {
                if (!(model.End <= e.Start || model.Start >= e.End))
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('This time is unavailable. Choose another time.';</script>");
                }
            }

            var calendarEvent = new Event
            {
                Start = model.Start,
                End = model.Start.AddMinutes(30),
                Text = $"Reservation to Doctor : " 
                    + (await _polyclinicContext.Doctors.FindAsync(model.DoctorId)).FName 
                    + " " 
                    + (await _polyclinicContext.Doctors.FindAsync(model.DoctorId)).LName
                    + "Patient : " + (await _polyclinicContext.Patients.FindAsync(model.PatientId)).FName 
                    + " " 
                    + (await _polyclinicContext.Patients.FindAsync(model.PatientId)).LName,
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
            };

            await _polyclinicContext.Events.AddAsync(calendarEvent);
            await _polyclinicContext.SaveChangesAsync();

            if (phoneNumber is not null)
            {
                var SmsStatus = await _smsManager.SendAsync(calendarEvent.Text, phoneNumber);
                ViewBag.Patient = await _polyclinicContext.Patients.FindAsync(model.PatientId);
                ViewBag.Doctor = await _polyclinicContext.Doctors.FindAsync(model.DoctorId);
                ViewBag.SMS = SmsStatus;
            }

            return View("ReservationInfo", model);
        }

        public async Task<IActionResult> CreateReservationPatientForm(Guid id)
        {
            if (User.IsInRole("Patient"))
            {
                var doctor = await _polyclinicContext.Doctors.FindAsync(id);
                var doctorViewModel = new DoctorViewModel
                {
                    Id = doctor.Id,
                    FName = doctor.FName,
                    LName = doctor.LName,
                };
                ViewBag.Doctor = doctorViewModel;
                ViewBag.Patient = await _polyclinicContext.Patients.FindAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            else
            {
                var patient = await _polyclinicContext.Patients.FindAsync(id);
                var patientModel = new PatientViewModel
                {
                    Id = patient.Id,
                    FName = patient.FName,
                    LName = patient.LName,
                };
                ViewBag.Patient = patientModel;
                ViewBag.Doctor = await _polyclinicContext.Doctors.FindAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            return View("~/Views/Visit/CreateReservationPatient.cshtml");
        }

        [HttpGet]
        public IActionResult CreateVisitDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitDoctor(VisitViewModel model, Guid patienSelect, string addPrescription)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            Visit visit = new()
            {
                Id = new Guid(),
                Description = model.Description,
                Date = model.Date,
                Diagnosis = model.Diagnosis,
                DoctorId = (Guid)model.DoctorId,
                PatientId = patienSelect,
            };

            var visitTracker = await _polyclinicContext.Visits.AddAsync(visit);
            if (visitTracker.State == EntityState.Added)
            {
                await _polyclinicContext.SaveChangesAsync();
                if (addPrescription == "yes")
                {
                    return RedirectToAction("AddPrescription", visit);
                }
                else
                {
                    return RedirectToAction("ShowMyVisits", "Doctor");
                }
            }

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public IActionResult AddPrescription(Visit visit)
        {
            ViewBag.Visit = visit;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionViewModel model, string chooseAction)
        {
            if (!ModelState.IsValid)
            {
                var visit = await _polyclinicContext.Visits.FindAsync(model.VisitId);
                ViewBag.Visit = visit;
                return View();
            }

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                Cure = model.Cure,
                DateOfPrescription = model.DateOfPrescription,
                Comment = model.Comment,
                ValidTill = model.ValidTill,
                DoctorId = model.DoctorId,
                VisitId = model.VisitId,
                PatientId = model.PatientId,
            };

            var prescriptionTracker = await _polyclinicContext.Prescriptions.AddAsync(prescription);
            if (prescriptionTracker.State == EntityState.Added)
            {
                await _polyclinicContext.SaveChangesAsync();
                if (chooseAction == "Save")
                {
                    return RedirectToAction("ShowMyVisits", "Doctor");
                }

                var visit = await _polyclinicContext.Visits.FindAsync(model.VisitId);
                return RedirectToAction("AddPrescription", "Visit", visit);
            }

            return Content("Internal error! Prescription Entity is not added.");
        }

        public async Task<IActionResult> PrescriptionInfo(Guid id)
        {
            var prescription = await _polyclinicContext.Prescriptions.FindAsync(id);
            if (prescription is null)
            {
                ViewBag.Message = "Error! No Prescription found.";
                return View();
            }

            var prescriptionViewModel = new PrescriptionViewModel
            {
                Id = prescription.Id,
                Cure = prescription.Cure,
                DateOfPrescription = prescription.DateOfPrescription,
                Comment = prescription.Comment,
                ValidTill = prescription.ValidTill,
                PatientId = prescription.PatientId,
                DoctorId = prescription.DoctorId,
                VisitId = prescription.VisitId,
            };

            ViewBag.Patient = await _polyclinicContext.Patients.FindAsync(prescriptionViewModel.PatientId);
            ViewBag.Doctor = await _polyclinicContext.Doctors.FindAsync(prescriptionViewModel.DoctorId);

            return View(prescriptionViewModel);
        }
    }
}
