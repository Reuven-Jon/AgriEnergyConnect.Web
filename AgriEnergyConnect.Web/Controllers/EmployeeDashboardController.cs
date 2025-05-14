using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AgriEnergyConnect.Web.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // I inject my DbContext so I can talk to the database,
        // and UserManager in case I need to know who’s signed in.
        public EmployeeDashboardController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /EmployeeDashboard/Dashboard
        // My main landing page for employees.
        public IActionResult Dashboard()
        {
            // I pull all products with their farmer info.
            var products = _context.Products
                                   .Include(p => p.Farmer)
                                   .ToList();

            // I load dropdown options for filters (even if I’m not filtering here).
            var categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Name, Text = c.Name })
                .ToList();

            var farmers = _context.Farmers
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.FullName })
                .ToList();

            // I build the ViewModel for the Dashboard view.
            var vm = new EmployeeDashboardViewModel
            {
                AllProducts = products,
                ProductFilter = new ProductFilterViewModel(),
                Categories = categories,
                Farmers = farmers
            };

            return View(vm);
        }

        // GET: /EmployeeDashboard/RegisterFarmer
        // I show a form for adding a new farmer.
        [HttpGet]
        public IActionResult RegisterFarmer()
        {
            return View();
        }

        // POST: /EmployeeDashboard/RegisterFarmer
        // I save that new farmer and bounce back to the dashboard.
        [HttpPost]
        public async Task<IActionResult> RegisterFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid)
                return View(farmer);

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Farmer '{farmer.FullName}' registered successfully.";
            return RedirectToAction("Dashboard");
        }

        // GET: /EmployeeDashboard/FilterProducts
        // I show the filter form (and current results if any).
        [HttpGet]
        public IActionResult FilterProducts()
        {
            var vm = new ProductFilterViewModel
            {
                Categories = _context.Categories
                                     .Select(c => new SelectListItem { Value = c.Name, Text = c.Name })
                                     .ToList(),
                Farmers = _context.Farmers
                                  .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.FullName })
                                  .ToList()
            };
            return View(vm);
        }

        // POST: /EmployeeDashboard/FilterProducts
        // I apply the user’s filters and redisplay the form with results.
        [HttpPost]
        public IActionResult FilterProducts(ProductFilterViewModel filter)
        {
            // Rebuild the dropdown lists so they persist
            filter.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.Name, Text = c.Name })
                .ToList();

            filter.Farmers = _context.Farmers
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.FullName })
                .ToList();

            // Apply the filters
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(p => p.Category == filter.Category);

            if (filter.FarmerId > 0)
                query = query.Where(p => p.FarmerId == filter.FarmerId);

            if (filter.FromDate.HasValue)
                query = query.Where(p => p.ProductionDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(p => p.ProductionDate <= filter.ToDate.Value);

            filter.Results = query.ToList();
            return View(filter);
        }

        // GET: /EmployeeDashboard/AllProducts
        // I let the employee see *every* product in the system.
        [HttpGet]
        public IActionResult AllProducts()
        {
            var products = _context.Products
                                   .Include(p => p.Farmer)
                                   .ToList();
            return View(products);
        }

        // GET: /EmployeeDashboard/ExportReport
        // I build a simple CSV and push it to the browser for download.
        [HttpGet]
        public IActionResult ExportReport()
        {
            var products = _context.Products
                                   .Include(p => p.Farmer)
                                   .ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Name,Category,ProductionDate,Farmer");

            foreach (var p in products)
            {
                var date = p.ProductionDate.ToString("yyyy-MM-dd");
                var farmer = p.Farmer?.FullName.Replace(",", ""); // avoid CSV break
                csv.AppendLine($"{p.Name},{p.Category},{date},{farmer}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "ProductsReport.csv");
        }
    }
}
