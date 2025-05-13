using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Web.Controllers
{
    public class CreateProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreateProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Farmers = _context.Farmers
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.FullName
                }).ToList();

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Product product)
        {
            if (product.Category == "0" || product.FarmerId == 0)
            {
                ModelState.AddModelError("", "Please select a valid Category and Farmer.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Farmers = _context.Farmers
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.FullName
                    }).ToList();

                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Name,
                        Text = c.Name
                    }).ToList();

                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "FarmerDashboard");
        }
    }
}
