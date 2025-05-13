using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // This will render Views/Home/Index.cshtml
        }
    }
}
