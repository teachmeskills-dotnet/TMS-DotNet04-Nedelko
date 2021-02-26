using DBTestCreator_1.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.Areas = _myContext.Areas.ToList();
            return View(_myContext.Patients.ToList());
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
            return View("/Views/Visit/CreateVisitDoctor.cshtml");
        }

    }
}
