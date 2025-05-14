
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriEnergyConnect.Web.Models
{
    public class ProductFilterViewModel
    {
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Display(Name = "Farmer")]
        public int FarmerId { get; set; }

        // These two power the dropdowns
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Farmers { get; set; } = new();

        // This holds the filtered results
        public List<Product> Results { get; set; } = new();
    }
}
