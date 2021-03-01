using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
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

        public PatientController(MyContext myContext)
        {
            _myContext = myContext;
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

    }
}
