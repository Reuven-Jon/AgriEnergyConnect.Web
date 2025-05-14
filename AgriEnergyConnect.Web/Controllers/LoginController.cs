using Microsoft.AspNetCore.Authentication.Cookies;      // for cookie schemes
using Microsoft.AspNetCore.Authentication;             // for AuthenticationProperties (if needed)
using Microsoft.AspNetCore.Identity;                   // for SignInManager & UserManager
using Microsoft.AspNetCore.Mvc;                        // for Controller & IActionResult
using AgriEnergyConnect.Web.Data;                      // for ApplicationDbContext
using AgriEnergyConnect.Web.Models;                    // for ForgotPasswordViewModel
using System.Threading.Tasks;                          // for async Task
using Microsoft.EntityFrameworkCore;                   // for EF async extensions

namespace AgriEnergyConnect.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;        // your EF context
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(
            ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        // GET: show login page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: attempt sign in
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // check credentials
            var result = await _signInManager
                .PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");  // on success go home

            ModelState.AddModelError("", "Login failed."); // show error
            return View();                                 // reload login
        }

        // GET: show forgot-password form
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); // this should return Views/Login/ForgotPassword.cshtml
        }


        // POST: update the user’s password just like registration
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // 1. Validate form
            if (!ModelState.IsValid)
                return View(model);

            // 2. Find user by Identity system
            var identityUser = await _userManager.FindByNameAsync(model.Username);
            if (identityUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            // 3. Generate a secure reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);

            // 4. Use the token to reset the password
            var result = await _userManager.ResetPasswordAsync(identityUser, token, model.NewPassword);

            if (result.Succeeded)
                return RedirectToAction("Index");

            // 5. Show validation errors
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

    }
}
