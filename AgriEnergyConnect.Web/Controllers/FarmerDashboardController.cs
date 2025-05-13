using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Web.Data;
using AgriEnergyConnect.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;

namespace AgriEnergyConnect.Web.Controllers
{
    public class FarmerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // I inject my DbContext and UserManager so I can load data and know who's logged in.
        public FarmerDashboardController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /FarmerDashboard/Dashboard
        // I gather everything I need for the dashboard: this user's products and any dropdown data.
        public IActionResult Dashboard()
        {
            // 1. Get the current user's ID so that I only show their products
            var userId = _userManager.GetUserId(User);

            // 2. Query the database for products that belong to me (The User), and include the Farmer relationship
            var products = _context.Products
                                   .Where(p => p.ApplicationUserId == userId)
                                   .Include(p => p.Farmer)
                                   .ToList();

            // 3. Optionally show a flash message if I just added a product
            if (TempData["SuccessMessage"] is string msg)
            {
                ViewData["SuccessMessage"] = msg;
            }

            // 4. Prepare category and farmer lists in case I need inline forms here
            var categories = _context.Categories
                                     .Select(c => new SelectListItem
                                     {
                                         Value = c.Name,
                                         Text = c.Name
                                     })
                                     .ToList();

            var farmers = _context.Farmers
                                  .Select(f => new SelectListItem
                                  {
                                      Value = f.Id.ToString(),
                                      Text = f.FullName
                                  })
                                  .ToList();

            // 5. Build my ViewModel with everything
            var viewModel = new DashboardViewModel
            {
                Products = products,
                NewProduct = new Product(),  // ready if I add inline creation
                Categories = categories,
                Farmers = farmers
            };

            // 6. Return the view with my assembled data
            return View(viewModel);
        }
    }
}

/*
 References for deeper understanding:
 - ASP.NET Core MVC controllers & actions: 
   https://docs.microsoft.com/aspnet/core/mvc/controllers/actions
 - Filtering EF Core queries by user: 
   https://docs.microsoft.com/ef/core/querying/basic
 - Passing TempData between requests:
   https://docs.microsoft.com/aspnet/core/fundamentals/app-state#tempdata
*/
