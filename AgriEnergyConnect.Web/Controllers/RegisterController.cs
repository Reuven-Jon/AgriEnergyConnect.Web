using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Models;
using AgriEnergyConnect.Web.Data;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(AppUser user)
        {
            if (!ModelState.IsValid) return View(user);

            user.Role = string.IsNullOrEmpty(user.Role) ? "Guest" : user.Role;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }
    }
}
