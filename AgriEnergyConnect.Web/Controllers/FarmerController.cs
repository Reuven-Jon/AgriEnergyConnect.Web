using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace AgriEnergyConnect.Web.Controllers
{
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // I inject my DbContext and UserManager so I can access data and the current user.
        public FarmerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Farmer/CreateProduct
        // I show the form for creating a new product.
        public IActionResult CreateProduct()
        {
            PopulateDropdowns();
            return View("~/Views/FarmerDashboard/CreateProduct.cshtml");
        }

        // POST: /Farmer/CreateProduct
        // I handle form submission, seed a new farmer if needed, tie product to my account, and save it.
        [HttpPost]
        public IActionResult CreateProduct(Product product, string? newFarmerName)
        {
            // 1. If the user types a new farmer name, I add it right away.
            if (!string.IsNullOrWhiteSpace(newFarmerName))
            {
                var newFarmer = new Farmer { FullName = newFarmerName };
                _context.Farmers.Add(newFarmer);
                _context.SaveChanges();
                product.FarmerId = newFarmer.Id;
            }

            // 2. I stamp the product with the current user's ID.
            product.ApplicationUserId = _userManager.GetUserId(User)!;

            // 3. If anything fails validation, I reload the form and show errors.
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("~/Views/FarmerDashboard/CreateProduct.cshtml", product);
            }

            // 4. I save the product and set a flash message for the dashboard.
            _context.Products.Add(product);
            _context.SaveChanges();
            TempData["SuccessMessage"] = $"I just saved '{product.Name}'!";

            // 5. Redirect me back to my dashboard so I can see the new entry.
            return RedirectToAction("Dashboard", "FarmerDashboard");
        }

        // I keep all dropdown seeding and ViewBag prep in one place.
        private void PopulateDropdowns()
        {
            // Seed default categories once
            if (!_context.Categories.Any())
            {
                var defaults = new[] { "Vegetables", "Fruits", "Red meat", "White meat", "Dairy", "Grains" };
                foreach (var name in defaults)
                    _context.Categories.Add(new Category { Name = name });
                _context.SaveChanges();
            }

            // Seed a few sample farmers if needed
            if (_context.Farmers.Count() < 3)
            {
                var sample = new[] { "Alice Grower", "Bob Harvester", "Cathy Rancher" };
                foreach (var f in sample)
                    if (!_context.Farmers.Any(x => x.FullName == f))
                        _context.Farmers.Add(new Farmer { FullName = f });
                _context.SaveChanges();
            }

            // Build dropdowns for the view
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Name, Text = c.Name })
                .ToList();

            ViewBag.Farmers = _context.Farmers
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.FullName })
                .ToList();
        }
    }
}
// Dependency injection patterns in ASP .NET Core: https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection

// Filtering EF queries by user: https://docs.microsoft.com/ef/core/querying/