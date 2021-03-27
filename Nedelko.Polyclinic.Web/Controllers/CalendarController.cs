using Microsoft.AspNetCore.Mvc;

namespace Nedelko.Polyclinic.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Show()
        {
            return View("~/Views/Event/Index.cshtml");
        }
    }
}
