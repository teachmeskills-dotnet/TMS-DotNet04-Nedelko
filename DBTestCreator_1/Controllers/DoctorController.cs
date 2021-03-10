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

        public async Task<IActionResult> ShowMyPatients()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myPatientsId = new HashSet<Guid>(_myContext.Visits.Where(v => v.DoctorId.ToString() == doctorId)
                .Select(visit => visit.PatientId));
            if (myPatientsId is null)
            {
                ViewBag.IsEmpty = "No Patients added yet.";
                return View();
            }
            else
            {
                var myPatients = new List<PatientModel>();
                foreach (var id in myPatientsId)
                {
                    var patientTemp = await _myContext.Patients.FindAsync(id);
                    myPatients.Add(new PatientModel
                    {
                        Id = patientTemp.Id,
                        FName = patientTemp.FName,
                        LName = patientTemp.LName,
                        Age = patientTemp.Age,
                        BDate = patientTemp.BDate,
                        EnumStatus = patientTemp.Status,
                        AreaId = patientTemp.AreaId
                    }
                        );
                }
                ViewBag.Areas = await _myContext.Areas.AsNoTracking().ToListAsync();
                return View(myPatients);
            }
        }

        public async Task<IActionResult> ShowMyVisits()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myVisits = _myContext.Visits.Where(v => v.DoctorId.ToString() == doctorId).Select(visit => new VisitModel
            {
                Id = visit.Id,
                DateOfVisit = visit.DateOfVisit,
                Diagnosis = visit.Diagnosis,
                PatientId = visit.PatientId,
                DoctorId = visit.DoctorId,
                Description = visit.Description,
            })
                .AsNoTracking().ToList();
            if(myVisits.Count() != 0)
            {
                ViewBag.Patients = await _myContext.Patients.AsNoTracking().ToListAsync();
                return View(myVisits);
            }
            else
            {
                ViewBag.Message = "NO Visits created yet.";
                return View(myVisits);
            }
            
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

        public async Task<IActionResult> Info(Guid id)
        {
            var doctor = await _myContext.Doctors.FindAsync(id);
            if(doctor is not null)
            {
                RegDoctorModel model = new RegDoctorModel
                {
                    Id = doctor.Id,
                    FName = doctor.FName,
                    LName = doctor.LName,
                    Position = RegDoctorModel.GetPosition(doctor.Code),
                    Department = doctor.DepartmentId,
                    Area = doctor.AreaId,
                };
                ViewBag.Areas = await _myContext.Areas.AsNoTracking().ToListAsync();
                ViewBag.Deps = await _myContext.Departments.AsNoTracking().ToListAsync();
                return View(model);
            }
            ViewBag.Message = "No Doctor found.";
            return View();
        }
    }
}
