using Core.Entities;
using Coreations_Task.ViewModels;
using Infrastructure;
using Infrastructure.Data.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coreations_Task.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManger;
        private readonly Context _context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            Context context)
        {
            _userManger = userManager;
            _signInManger = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);

            var user = await _userManger.FindByEmailAsync(login.EmailAddress);

            if (user != null)
            {
                var passCheck = await _userManger.CheckPasswordAsync(user, login.Password);
                if (passCheck)
                {
                    var result = await _signInManger.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index","Product");
                    }
                }
                TempData["Error"] = "Wrong User. Please try again!";
                return View(login);
            }
            TempData["Error"] = "Wrong User. Please try again!";
            return View(login);
        }

        public IActionResult Register()
        {
            var response = new ReigsterVM();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(ReigsterVM reg)
        {
            if (!ModelState.IsValid) return View(reg);
            var user = await _userManger.FindByEmailAsync(reg.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(reg);
            }
            var newUser = new User()
            {
                FullName = reg.FullName,
                Email = reg.EmailAddress,
                UserName = reg.EmailAddress
            };
            var newUserResponse = await _userManger.CreateAsync(newUser, reg.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManger.AddToRoleAsync(newUser, Roles.User);
            }
            return View("RegisterCompleted");

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }
    }
}
