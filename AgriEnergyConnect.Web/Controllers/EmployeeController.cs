using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;

namespace AgriEnergyConnect.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult RegisterFarmer() => View();

        [HttpPost]
        public IActionResult RegisterFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid)
                return View(farmer);

            _context.Farmers.Add(farmer);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}