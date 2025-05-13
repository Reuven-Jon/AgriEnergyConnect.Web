using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Web.Controllers
{
    public class RegisterFarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterFarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Farmer farmer)
        {
            if (!ModelState.IsValid)
            {
                return View(farmer);
            }

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "EmployeeDashboard");
        }
    }
}
