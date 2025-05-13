using AgriEnergyConnect.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{


    public class Farmer
    {
        public int Id { get; set; } // maps to identify User ID

        [Required]
        public string FullName { get; set; } = null!; // Not nullable

        [EmailAddress]
        public string? Email { get; set; } // Nullable if optional

        public string? PhoneNumber { get; set; } // Nullable if optional

        public string? Address { get; set; }

        public ICollection<Product>? Products { get; set; } // Navigation property, can be nullable
    }
}