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

        [HttpGet]
        public IActionResult CreateVisitDoctor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitDoctor(VisitModel model, Guid patienSelect)
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
                    await _myContext.SaveChangesAsync();
                }
            }
            return View();
        }
    }
}
