using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // This will render Views/Home/Index.cshtml
        }
        // Controllers/HomeController.cs
        public IActionResult Privacy()
            => RedirectToAction("Index", "Chat");
        public IActionResult LearnMore()
   => View();

        public IActionResult MarketPlace()
        {
            return View();
        }



    }
}
