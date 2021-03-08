﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Show(Guid id)
        {
            ViewBag.Doctor = id.ToString();
            return View("~/Views/Event/Index.cshtml");
        }
    }
}