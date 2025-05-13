using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, MinLength(10)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Guest"; // Farmer, Employee, Guest

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
