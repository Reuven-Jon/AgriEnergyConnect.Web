using System.Linq;                             // for OrderBy
using System.Threading.Tasks;                  // for Task
using AgriEnergyConnect.Web.Data;              // for ApplicationDbContext
using AgriEnergyConnect.Web.Models;            // for ChatMessage
using Microsoft.AspNetCore.Mvc;                // for Controller & IActionResult
using Microsoft.EntityFrameworkCore;           // for ToListAsync()

namespace AgriEnergyConnect.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Constructor: inject EF Core DbContext
        public ChatController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /Chat or /Chat/Index
        public async Task<IActionResult> Index()
        {
            // 1. Fetch all chat messages, oldest first
            var messages = await _db.ChatMessages
                                    .OrderBy(m => m.Timestamp)
                                    .ToListAsync();

            // 2. Pass the list into Views/Chat/Index.cshtml
            return View(messages);
        }
    }
}
