using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Models;
using Nedelko.Polyclinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    public class DoctorController : Controller
    {
        private readonly PolyclinicContext _polyclinicContext;
        private readonly UserManager<User> _userManager;

        public DoctorController(
            PolyclinicContext polyclinicContext, 
            UserManager<User> userManager)
        {
            _polyclinicContext = polyclinicContext ?? throw new ArgumentNullException(nameof(polyclinicContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> ShowMyPatients()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patientIds =
                new HashSet<Guid>(_polyclinicContext.Visits
                    .Where(visit => visit.DoctorId.ToString() == doctorId)
                .Select(visit => visit.PatientId));

            if (patientIds is null)
            {
                ViewBag.IsEmpty = "No patients added yet.";
                return View();
            }

            var patientViewModels = new List<PatientViewModel>();
            foreach (var id in patientIds)
            {
                var patientTemp = await _polyclinicContext.Patients.FindAsync(id);
                patientViewModels.Add(new PatientViewModel
                {
                    Id = patientTemp.Id,
                    FName = patientTemp.FName,
                    LName = patientTemp.LName,
                    Age = patientTemp.Age,
                    BDate = patientTemp.BDate,
                    EnumStatus = patientTemp.Status,
                    AreaId = patientTemp.AreaId
                });
            }

            ViewBag.Areas =
                await _polyclinicContext.Areas
                .AsNoTracking()
                .ToListAsync();

            return View(patientViewModels);
        }

        public async Task<IActionResult> ShowMyVisits()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var visits = await _polyclinicContext.Visits
                .Where(visit => visit.DoctorId.ToString() == doctorId)
                .Select(visit => new VisitViewModel
                {
                    Id = visit.Id,
                    Date = visit.Date,
                    Diagnosis = visit.Diagnosis,
                    PatientId = visit.PatientId,
                    DoctorId = visit.DoctorId,
                    Description = visit.Description,
                })
                .AsNoTracking()
                .ToListAsync();

            if (!visits.Any())
            {
                ViewBag.Message = "No visits created yet.";
                return View(visits);
            }

            ViewBag.Patients =
                await _polyclinicContext.Patients
                .AsNoTracking()
                .ToListAsync();

            return View(visits);
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            ViewBag.Deps = 
                await _polyclinicContext.Departments
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            return View(await _polyclinicContext.Doctors.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var doctor = await _polyclinicContext.Doctors.FindAsync(id);
            if (doctor is null)
            {
                return Content("Item not found.");
            }

            var doctorViewModel = new DoctorViewModel
            {
                Id = doctor.Id,
                FName = doctor.FName,
                LName = doctor.LName,
                Age = doctor.Age,
                HiredDate = doctor.HiredDate,
                Department = doctor.DepartmentId,
                Area = doctor.AreaId,
            };

            ViewBag.Deps =
                await _polyclinicContext.Departments
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();
            
            return View(doctorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorViewModel model)
        {
            var doctor = await _polyclinicContext.Doctors.FindAsync(model.Id);
            if (doctor is null)
            {
                return Content("Item not found.");
            }

            doctor.FName = model.FName;
            doctor.LName = model.LName;
            doctor.Age = model.Age;
            doctor.HiredDate = model.HiredDate;
            doctor.DepartmentId = model.Department;
            doctor.AreaId = model.Area;

            await _polyclinicContext.SaveChangesAsync();

            

            return RedirectToAction("Show", "Doctor");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var doctor = await _polyclinicContext.Doctors.FindAsync(id);

            var regDoctorModel = new DoctorViewModel
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

            ViewBag.Deps =
                await _polyclinicContext.Departments
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            return View(regDoctorModel);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(DoctorViewModel model)
        {
            var doctor = await _polyclinicContext.Doctors.FindAsync(model.Id);
            if (doctor is null)
            {
                ViewBag.RemoveDoc = $"Doctor: { doctor.FName} { doctor.LName} is not found.";
                return View("RemoveDoctorResult");
            }

            var doctorTracker = _polyclinicContext.Doctors.Remove(doctor);
            if (doctorTracker.State == EntityState.Deleted)
            {
                await _polyclinicContext.SaveChangesAsync();
                ViewBag.RemoveDoc = $"Doctor : {doctor.FName} {doctor.LName} deleted.";
            }
            else
            {
                ViewBag.RemoveDoc = "Internal Error. Try again.";
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user is null)
            {
                ViewBag.RemoveUser = "User is not found.";
                return View("RemoveDoctorResult");
            }

            var identityResult = await _userManager.DeleteAsync(user);
            if (identityResult.Succeeded)
            {
                ViewBag.RemoveUser = "User entity deleted.";
            }
            else
            {
                ViewBag.RemoveUser = "User entity is not deleted.";
            }

            return View("RemoveDoctorResult");
        }

        public async Task<IActionResult> Info(Guid id)
        {
            var doctor = await _polyclinicContext.Doctors.FindAsync(id);
            if (doctor is null)
            {
                ViewBag.Message = "Doctor not found.";
                return View();
            }

            var doctorViewModel = new DoctorViewModel
            {
                Id = doctor.Id,
                FName = doctor.FName,
                LName = doctor.LName,
                Position = DoctorViewModel.GetPosition(doctor.Code),
                Department = doctor.DepartmentId,
                Area = doctor.AreaId,
            };

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Deps =
                await _polyclinicContext.Departments
                    .AsNoTracking()
                    .ToListAsync();

            return View(doctorViewModel);
        }
    }
}
