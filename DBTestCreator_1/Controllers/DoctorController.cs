using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class DoctorController : Controller
    {
        private readonly MyContext _myContext;
        private readonly UserManager<User> _usermanager;

        public DoctorController(MyContext myContext, UserManager<User> usermanager)
        {
            _myContext = myContext;
            _usermanager = usermanager;

        }

        public IActionResult ShowMyVisits()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myVisits = _myContext.Visits.Where(v => v.DoctorId.ToString() == doctorId).Select(visit => new VisitModel
            {
                Id = visit.Id,
                DateOfVisit = visit.DateOfVisit,
                Diagnosis = visit.Diagnosis,
                PatientId = visit.PatientId,
                Description = visit.Description,
            })
                .AsNoTracking().ToList();
            return View(myVisits);
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

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            Doctor doctor = await _myContext.Doctors.FindAsync(id);
            ViewBag.Deps = _myContext.Departments.ToList();
            ViewBag.Areas = _myContext.Areas.ToList();
            RegDoctorModel regDoctorModel = new RegDoctorModel
            {
                Id = doctor.Id,
                FName = doctor.FName,
                LName = doctor.LName,
                Age = doctor.Age,
                Code = doctor.Code,
                HiredDate = doctor.HiredDate,
                Department = doctor.DepartmentId,
                Area = doctor.AreaId,
            };
            return View(regDoctorModel);

        }
        [HttpPost]
        public async Task<IActionResult> Remove(RegDoctorModel model)
        {
            Doctor doctor = await _myContext.Doctors.FindAsync(model.Id);
            User user = await _usermanager.FindByIdAsync(model.Id.ToString());
            if(doctor is not null)
            {
                var removeDoc = _myContext.Doctors.Remove(doctor);
                if(removeDoc.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                {
                    await _myContext.SaveChangesAsync();
                    ViewBag.RemoveDoc = $"Doctor : {doctor.FName} {doctor.LName} deleted.";
                }
                else
                {
                    ViewBag.RemoveDoc = "Internal Error. Try again.";
                }
                if (user is not null)
                {
                    var removeUser = await _usermanager.DeleteAsync(user);
                    if (removeUser.Succeeded)
                    {
                        ViewBag.RemoveUser = "User entity deleted.";
                    }
                    else
                    {
                        ViewBag.RemoveUser = "User entity is NOT deleted.";
                    }
                }
                else
                {
                    ViewBag.RemoveUser = "User is NOT found.";
                }
            }
            else
            {
                ViewBag.RemoveDoc = $"Doctor: { doctor.FName} { doctor.LName} is NOT found.";
            }
            return View("RemoveDoctorResult");
        }
    }
}
