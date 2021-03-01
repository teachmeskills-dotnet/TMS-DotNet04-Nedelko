using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class VisitController : Controller
    {
        private readonly MyContext _myContext;

        public VisitController(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<IActionResult> Info(Guid id)
        {
            var visit = await _myContext.Visits.FindAsync(id);
            VisitModel model = new VisitModel
            {
                Description = visit.Description,
                DateOfVisit = visit.DateOfVisit,
                Diagnosis = visit.Diagnosis,
            };
            ViewBag.Patient = await _myContext.Patients.FindAsync(visit.PatientId);
            ViewBag.Doctor = await _myContext.Doctors.FindAsync(visit.DoctorId);
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateVisitDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitDoctor(VisitModel model, Guid patienSelect, string addPrescription)
        {
            if (ModelState.IsValid)
            {
                Visit visit = new Visit
                {
                    Id = new Guid(),
                    Description = model.Description,
                    DateOfVisit = model.DateOfVisit,
                    Diagnosis = model.Diagnosis,
                    DoctorId = (Guid)model.DoctorId,
                    PatientId = patienSelect,
                };
                var addVisit = await _myContext.Visits.AddAsync(visit);
                if (addVisit.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    if(addPrescription == "YES")
                    {
                        return View("~/Views/Visit/AddPrescription.cshtml", visit);
                    }
                    else
                    {
                        await _myContext.SaveChangesAsync();
                        return RedirectToAction("ShowMyVisits", "Doctor");
                    } 
                }
            }
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
