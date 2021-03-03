using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class PatientController : Controller
    {
        private readonly MyContext _myContext;
        private readonly UserManager<User> _userManager;

        public PatientController(MyContext myContext, UserManager<User> userManager)
        {
            _myContext = myContext;
            _userManager = userManager;
        }


        public async Task<IActionResult> Detailes(Guid id)
        {
            var patient = await _myContext.Patients.FindAsync(id);
            var patientModel = new PatientModel
            {
                FName = patient.FName,
                LName = patient.LName,
                Age = patient.Age,
                BDate = patient.BDate,
                EnumStatus = patient.Status,
                AreaId = patient.AreaId,
            };
            ViewBag.Address = await _myContext.PatientAddresses.FindAsync(id);
            ViewBag.Areas = await _myContext.Areas.AsNoTracking().ToListAsync();
            ViewBag.Email = (await _userManager.FindByIdAsync(id.ToString())).Email;
            return View(patientModel);
        }

        public async Task<IActionResult> Show()
        {
            ViewBag.Areas = await _myContext.Areas.AsNoTracking().ToListAsync();
            return View(await _myContext.Patients.AsNoTracking().ToListAsync());
        }

        public IActionResult Find(string patientToFind)
        {
            var pat = _myContext.Patients.ToList();
            List<Patient> patients = new List<Patient>();
            foreach(var p in pat)
            {
                if(p.LName == patientToFind)
                {
                    patients.Add(p);
                }
            }
            ViewBag.PatFind = patients;
            ViewBag.Doctor = User.Identity.Name;
            return View("/Views/Visit/CreateVisitDoctor.cshtml");
        }

        public async Task<IActionResult> ShowMyVisits(Guid id)
        {
            var result = await _myContext.Visits.AsNoTracking()
                .Where(v => v.PatientId == id).ToListAsync();
            List<Doctor> myDoctors = new List<Doctor>();
            if(result.Count() != 0)
            {
                var patient = await _myContext.Patients.FindAsync(id);
                ViewBag.PatientName = String.Concat(patient.FName, "", patient.LName);
                foreach(var v in result)
                {
                    myDoctors.Add(await _myContext.Doctors.FindAsync(v.DoctorId));
                }
                ViewBag.Doctors = myDoctors;
                return View(result);
            }
            else
            {
                ViewBag.Message = "NO Visits were found.";
                return View();
            }
        }

    }
}
