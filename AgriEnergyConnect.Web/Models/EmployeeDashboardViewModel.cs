using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriEnergyConnect.Web.Models
{
    public class EmployeeDashboardViewModel
    {
        // I hold the full list of products after filtering
        public List<Product> AllProducts { get; set; } = new();

        // I use this to bind filter inputs from the UI
        public ProductFilterViewModel ProductFilter { get; set; } = new();

        // These are used to populate dropdowns in the filter form
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Farmers { get; set; } = new();
    }
}
