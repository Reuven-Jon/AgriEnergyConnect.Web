using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public string? Description { get; set; }

        [Required]
        public int FarmerId { get; set; }

        public Farmer? Farmer { get; set; }

        //  I don’t want this bound or validated from the form
        [BindNever]
        [ValidateNever]
        public string ApplicationUserId { get; set; } = null!;

        //  This gets set date automatically when I save the product
        [BindNever]
        [ValidateNever]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
