using System.ComponentModel.DataAnnotations;


namespace AgriEnergyConnect.Web.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime ProductionDate { get; set; }
        public string? Description { get; set; }

        // Link to Farmer (optional, if you still want that)
        public int FarmerId { get; set; }
        public Farmer? Farmer { get; set; }

        // New: link to the Identity user who created it
        public string ApplicationUserId { get; set; } = null!;
    }
}