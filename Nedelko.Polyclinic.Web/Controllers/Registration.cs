using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Models;
using Nedelko.Polyclinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    public class Registration : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PolyclinicContext _polyclinicContext;

        public Registration(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            PolyclinicContext polyclinicContext)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new System.ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new System.ArgumentNullException(nameof(roleManager));
            _polyclinicContext = polyclinicContext ?? throw new System.ArgumentNullException(nameof(polyclinicContext));
        }

        [HttpGet]
        public async Task<IActionResult> PatientRegistrationAsync(User user)
        {
            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Id = user.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PatientRegistration(PatientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var patient = new Patient
            {
                Id = model.Id,
                FName = model.FName,
                LName = model.LName,
                Age = model.Age,
                BDate = model.BDate,
                AreaId = model.AreaId,
                Status = PatientViewModel.GetStatus(model.Status),
            };

            var patientTracker = await _polyclinicContext.Patients.AddAsync(patient);
            if (patientTracker.State == EntityState.Added)
            {
                await _polyclinicContext.SaveChangesAsync();
                ViewBag.AddPatient = "New Patient Added.";
            }
            else
            {
                ViewBag.AddPatient = "New Patient Failed.";
            }

            ViewBag.Id = model.Id;
            return View("PatientAddress");
        }

        [HttpPost]
        public async Task<IActionResult> PatientAddress(PatientAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AddressResult = "Model is NOT valid.";
                return View();
            }

            var patientAddress = new PatientAddress
            {
                Id = model.Id,
                City = model.City,
                Street = model.Street,
                Number = model.Number,
                PatientId = model.Id,
            };

            var addressTracker = await _polyclinicContext.PatientAddresses.AddAsync(patientAddress);
            if (addressTracker.State == EntityState.Added)
            {
                await _polyclinicContext.SaveChangesAsync();
            }
            else
            {
                ViewBag.AddressResult = "Address NOT saved.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> DoctorRegistrationAsync(User user)
        {
            ViewBag.Deps = 
                await _polyclinicContext.Departments
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Areas =
                await _polyclinicContext.Areas
                    .AsNoTracking()
                    .ToListAsync();

            ViewBag.Id = user.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegistration(DoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Id = model.Id,
                    Age = model.Age,
                    FName = model.FName,
                    LName = model.LName,
                    Code = model.Code,
                    HiredDate = model.HiredDate,
                    DepartmentId = model.Department,
                    AreaId = model.Area,
                };

                var doctorTracker = await _polyclinicContext.Doctors.AddAsync(doctor);
                if (doctorTracker.State == EntityState.Added)
                {
                    ViewBag.AddDoctor = "New Doctor Added.";
                    await _polyclinicContext.SaveChangesAsync();
                }
                else
                {
                    ViewBag.AddDoctor = "Doctor failed.";
                }
            }
            else
            {
                ViewBag.CreateDoctor = "Model is Invalid.";
            }

            return View("DoctorResult");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Error! Try again.");
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user is null)
            {
                ViewData["Error"] = "Error! User not found.";
                return View();
            }

            var signInResult =
                await _signInManager.PasswordSignInAsync(
                    user, 
                    loginModel.Password, 
                    false, 
                    false);

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Error"] = "Error! Login failed.";
            return View();
        }

        public async Task<IActionResult> LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UserRegistration(UserViewModel userModel, string userType)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (ModelState.IsValid && user is null)
            {
                user = new User
                {
                    UserName = userModel.Name,
                    Email = userModel.Email,
                };

                var identityResult = await _userManager.CreateAsync(user, userModel.Password);
                if (identityResult.Succeeded)
                {
                    if (await _roleManager.FindByNameAsync(userType) is null)
                    {
                        var userRole = new IdentityRole { Name = userType };
                        await _roleManager.CreateAsync(userRole);
                    }

                    await _userManager.AddToRoleAsync(user, userType);

                    if (User.IsInRole("Administrator"))
                    {
                        return RedirectToAction(userType + "Registration", "Registration", user);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Message = "User with the same Name or Email is also registered.";
            return View("UserRegistration");
        }
    }
}
