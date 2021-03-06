using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                PatientId = visit.PatientId,
            };
            ViewBag.Patient = await _myContext.Patients.FindAsync(visit.PatientId);
            ViewBag.Doctor = await _myContext.Doctors.FindAsync(visit.DoctorId);
            ViewBag.Prescriptions = await _myContext.Prescriptions.AsNoTracking()
                .Where(presc => presc.VisitId == id).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> CreateReservationPatient(ReservationModel model)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    Description = "None",
                    DateReservation = model.DateReservasion,
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                };
                await _myContext.Reservations.AddAsync(reservation);
                await _myContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task<IActionResult> CreateReservationPatientForm(Guid docItem)
        {
            var doctor = await _myContext.Doctors.FindAsync(docItem);
            var model = new RegDoctorModel
            {
                Id = doctor.Id,
                FName = doctor.FName,
                LName = doctor.LName,
            };
            ViewBag.Doctor = model;
            return View("~/Views/Visit/CreateReservationPatient.cshtml");
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
                    await _myContext.SaveChangesAsync();
                    if(addPrescription == "yes")
                    {
                        return RedirectToAction("AddPrescription", visit);
                    }
                    else
                    {
                        return RedirectToAction("ShowMyVisits", "Doctor");
                    }
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
        public async Task<IActionResult> AddPrescription(PrescriptionModel model, string chooseAction)
        {
            if (ModelState.IsValid)
            {
                Prescription prescription = new Prescription
                {
                    Id = Guid.NewGuid(),
                    Cure = model.Cure,
                    DateOfPrescription = model.DateOfPrescription,
                    Comments = model.Comments,
                    ValidTill = model.ValidTill,
                    DoctorId = model.DoctorId,
                    VisitId = model.VisitId,
                    PatientId = model.PatientId,
                };
                var addPrescription = await _myContext.Prescriptions.AddAsync(prescription);
                if(addPrescription.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    await _myContext.SaveChangesAsync();
                    if(chooseAction == "Save")
                    {
                        return RedirectToAction("ShowMyVisits", "Doctor");
                    }
                    else
                    {
                        var visit = await _myContext.Visits.FindAsync(model.VisitId);
                        return RedirectToAction("AddPrescription", "Visit", visit);
                    }
                }
                else
                {
                    return Content("Internal error! Prescription Entity is NOT added.");
                }
            }
            else
            {
                var visit = await _myContext.Visits.FindAsync(model.VisitId);
                ViewBag.Visit = visit;
                return View();
            }
        }

        public async Task<IActionResult> PrescriptionInfo(Guid id)
        {
            var model = await _myContext.Prescriptions.FindAsync(id);
            if(model is not null)
            {
                PrescriptionModel prescription = new PrescriptionModel
                {
                    Id = model.Id,
                    Cure = model.Cure,
                    DateOfPrescription = model.DateOfPrescription,
                    Comments = model.Comments,
                    ValidTill = model.ValidTill,
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                    VisitId = model.VisitId,
                };
                ViewBag.Patient = await _myContext.Patients.FindAsync(prescription.PatientId);
                ViewBag.Doctor = await _myContext.Doctors.FindAsync(prescription.DoctorId);
                return View(prescription);
            }
            else
            {
                ViewBag.Message = "Error! NO Prescription found.";
                return View();
            }
        }
    }
}
