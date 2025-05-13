using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AgriEnergyConnect.Web.Models
{
    public class DashboardViewModel
    {
        // For listing
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        // For the inline form (if you want it on the same page)
        public Product NewProduct { get; set; } = new Product();

        // Dropdowns
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Farmers { get; set; } = new();
    }
}
