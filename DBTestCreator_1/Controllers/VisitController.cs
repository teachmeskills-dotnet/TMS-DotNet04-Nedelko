using DBTestCreator_1.Managers;
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

        public async Task<IActionResult> CreateReservationPatient(CalendarEventModel model)
        {
            if (ModelState.IsValid)
            {
                var events = _myContext.Events.AsNoTracking()
                    .Where(e => e.DoctorId == model.DoctorId).ToList();
                model.End = model.Start.AddMinutes(30);
                foreach(var e in events)
                {
                    if(!(model.End <= e.Start || model.Start >= e.End))
                    {
                        return Content("This time is unavailable. Choose another time.");
                    }
                }
                CalendarEvent reservation = new CalendarEvent
                {
                    Start = model.Start,
                    End = model.Start.AddMinutes(30),
                    Text = $"Reservation to Doctor : " +  (await _myContext.Doctors.FindAsync(model.DoctorId)).FName + " " + (await _myContext.Doctors.FindAsync(model.DoctorId)).LName
                    + "Patient : " + (await _myContext.Patients.FindAsync(model.PatientId)).FName + " " + (await _myContext.Patients.FindAsync(model.PatientId)).LName,
                    PatientId = model.PatientId,
                    DoctorId = model.DoctorId,
                };
                await _myContext.Events.AddAsync(reservation);
                await _myContext.SaveChangesAsync();
                SMSManager smsManager = new SMSManager();
                var SMSStatus = await smsManager.SendSMSAsync(reservation.Text);
                ViewBag.Patient = await _myContext.Patients.FindAsync(model.PatientId);
                ViewBag.Doctor = await _myContext.Doctors.FindAsync(model.DoctorId);
                ViewBag.SMS = SMSStatus.Message;
                return View("ReservationInfo", model);
            }
            return View();
        }
        public async Task<IActionResult> CreateReservationPatientForm(Guid id)
        {
            if (User.IsInRole("Patient"))
            {
                var doctor = await _myContext.Doctors.FindAsync(id);
                var doctorModel = new RegDoctorModel
                {
                    Id = doctor.Id,
                    FName = doctor.FName,
                    LName = doctor.LName,
                };
                ViewBag.Doctor = doctorModel;
                ViewBag.Patient = await _myContext.Patients.FindAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            else
            {
                var patient = await _myContext.Patients.FindAsync(id);
                var patientModel = new PatientModel
                {
                    Id = patient.Id,
                    FName = patient.FName,
                    LName = patient.LName,
                };
                ViewBag.Patient = patientModel;
                ViewBag.Doctor = await _myContext.Doctors.FindAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

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
