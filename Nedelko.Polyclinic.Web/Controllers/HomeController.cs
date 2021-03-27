using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Models;
using Nedelko.Polyclinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PolyclinicContext _polyclinicContext;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            PolyclinicContext polyclinicContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _polyclinicContext = polyclinicContext ?? throw new ArgumentNullException(nameof(polyclinicContext));
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public async Task<IActionResult> Calendar()
        {
            ViewBag.Doctors =
                await _polyclinicContext.Doctors
                    .AsNoTracking()
                    .ToListAsync();

            return View("~/Views/Event/Index.cshtml");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
