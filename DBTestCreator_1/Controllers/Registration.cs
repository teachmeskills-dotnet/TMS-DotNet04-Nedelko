using DBTestCreator_1.Enums;
using DBTestCreator_1.Models;
using DBTestCreator_1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    public class Registration : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyContext _myContext;

        //private readonly HttpContext _httpContext;

        public Registration(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, MyContext myContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _myContext = myContext;
        }

        //PATIENT
        [HttpGet]
        public IActionResult PatientRegistration(User user)
        {
            ViewBag.Areas = _myContext.Areas.ToList();
            ViewBag.Id = user.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PatientRegistration(PatientModel model)
        {
            if (ModelState.IsValid)
            {
                Patient patient = new Patient
                {
                    Id = model.Id,
                    FName = model.FName,
                    LName = model.LName,
                    Age = model.Age,
                    BDate = model.BDate,
                    AreaId = model.AreaId,
                    Status = PatientModel.GetStatus(model.Status),
                };
                var addPatient = await _myContext.Patients.AddAsync(patient);
                if (addPatient.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    await _myContext.SaveChangesAsync();
                    ViewBag.AddPatient = "New Patient Added.";
                }
                else
                {
                    ViewBag.AddPatient = "New Patient Failed.";
                }
            }
            else
            {
                return View();
            }
            ViewBag.Id = model.Id;
            return View("PatientAddress");
        }

        [HttpPost]
        public async Task<IActionResult> PatientAddress(PatientAddressModel model)
        {
            if (ModelState.IsValid)
            {
                PatientAddress patientAddress = new PatientAddress
                {
                    Id = model.Id,
                    City = model.City,
                    Street = model.Street,
                    HouseNo = model.HouseNo,
                    PatientId = model.Id,
                };
                var addAddress = await _myContext.PatientAddresses.AddAsync(patientAddress);
                if(addAddress.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    await _myContext.SaveChangesAsync();
                }
                else
                {
                    ViewBag.AddressResult = "Address NOT saved.";
                }
            }
            else
            {
                ViewBag.AddressResult = "Model is NOT valid.";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // DOCTOR

        [HttpGet]
        public IActionResult DoctorRegistration(User user)
        {
            ViewBag.Deps = _myContext.Departments.ToList();
            ViewBag.Areas = _myContext.Areas.ToList();
            ViewBag.Id = user.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegistration(RegDoctorModel model)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = new Doctor
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
                var addDoctor = await _myContext.Doctors.AddAsync(doctor);

                if (addDoctor.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    ViewBag.AddDoctor = "New Doctor Added.";
                    await _myContext.SaveChangesAsync();
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
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = _userManager.Users.FirstOrDefault(u => u.Email == loginModel.Email);
                if(user == null)
                {
                    ViewData["Error"] = "Error! No USER found.";
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Error"] = "Error! Login failed.";
                    return View();
                }
            }
            else
            {
                return Content("Error! Try again.");
            }
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
        // ADMIN

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult UserRegistration()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UserRegistration(UserModel userModel, string userType)
        {
            if (ModelState.IsValid && await _userManager.FindByEmailAsync(userModel.Email) == null)
            {
                User user = new User
                {
                    UserName = userModel.Name,
                    Email = userModel.Email,
                };
                var addUser = await _userManager.CreateAsync(user, userModel.Password);
                if (addUser.Succeeded)
                {
                    if (await _roleManager.FindByNameAsync(userType) == null)
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
            return View("UserRegistration");
        }
    }
}
