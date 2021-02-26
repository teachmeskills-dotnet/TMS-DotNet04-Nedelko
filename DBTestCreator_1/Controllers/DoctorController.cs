using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class DoctorController : Controller
    {
        private readonly MyContext _myContext;

        public DoctorController(MyContext myContext)
        {
            _myContext = myContext;
        }

        public IActionResult MyVisits()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Show()
        {
            ViewBag.Deps = _myContext.Departments.ToList();
            ViewBag.Areas = _myContext.Areas.ToList();
            return View(_myContext.Doctors.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Doctor doctor = await _myContext.Doctors.FindAsync(id);
            if(doctor != null)
            {
                RegDoctorModel doctorModel = new RegDoctorModel
                {
                    Id = doctor.Id,
                    FName = doctor.FName,
                    LName = doctor.LName,
                    Age = doctor.Age,
                    HiredDate = doctor.HiredDate,
                    Department = doctor.DepartmentId,
                    Area = doctor.AreaId,

                };
                ViewBag.Deps = _myContext.Departments.ToList();
                ViewBag.Areas = _myContext.Areas.ToList();
                return View(doctorModel);
            }
            return Content("Item not found.");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegDoctorModel model)
        {
            var doctor = await _myContext.Doctors.FindAsync(model.Id);
            if (doctor != null)
            {
                doctor.FName = model.FName;
                doctor.LName = model.LName;
                doctor.Age = model.Age;
                doctor.HiredDate = model.HiredDate;
                doctor.DepartmentId = model.Department;
                doctor.AreaId = model.Area;
                await _myContext.SaveChangesAsync();
            }
            else
            {
                return Content("Item NOT found.");
            }
            ViewBag.Deps = _myContext.Departments.ToList();
            ViewBag.Areas = _myContext.Areas.ToList();
            return RedirectToAction("Show", "Doctor");
        }
    }
}
