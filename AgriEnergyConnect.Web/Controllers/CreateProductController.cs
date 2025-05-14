using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Web.Controllers
{
    public class CreateProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // I inject both DbContext and UserManager
        public CreateProductController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /CreateProduct/Index
        // I show the blank form with dropdowns.
        [HttpGet]
        public IActionResult Index()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: /CreateProduct/Index
        // I handle the submitted form, optional new farmer, stamp the user, save, and redirect.
        [HttpPost]
        public async Task<IActionResult> Index(Product product, string? newFarmerName)
        {
            // 1. If user entered a new farmer, add it now.
            if (!string.IsNullOrWhiteSpace(newFarmerName))
            {
                var nf = new Farmer { FullName = newFarmerName };
                _context.Farmers.Add(nf);
                await _context.SaveChangesAsync();
                product.FarmerId = nf.Id;
            }

            // 2. Stamp the product with the current user's ID.
            product.ApplicationUserId = _userManager.GetUserId(User)!;

            // 3. Validate. If it fails, re-show the form (with dropdowns).
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(product);
            }

            // 4. Save the product.
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // 5. Redirect me back to my dashboard so I can see the new entry.
            return RedirectToAction("Dashboard", "FarmerDashboard");
        }

        // I keep dropdown seeding in one place.
        private void PopulateDropdowns()
        {
            if (!_context.Categories.Any())
            {
                var defaults = new[] { "Vegetables", "Fruits", "Red meat", "White meat", "Dairy", "Grains" };
                foreach (var name in defaults)
                    _context.Categories.Add(new Category { Name = name });
                _context.SaveChanges();
            }

            if (_context.Farmers.Count() < 3)
            {
                var sample = new[] { "Alice Grower", "Bob Harvester", "Cathy Rancher" };
                foreach (var full in sample)
                    if (!_context.Farmers.Any(f => f.FullName == full))
                        _context.Farmers.Add(new Farmer { FullName = full });
                _context.SaveChanges();
            }

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Name, Text = c.Name })
                .ToList();

            ViewBag.Farmers = _context.Farmers
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.FullName })
                .ToList();
        }
    }
}
