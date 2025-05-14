using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Web.Models;

namespace AgriEnergyConnect.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Data/ApplicationDbContext.cs
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Seed two farmers
            builder.Entity<Farmer>().HasData(
                new Farmer
                {
                    Id = 1,
                    FullName = "Alice Green",
                    Email = "alice@farm.co.za"
                },
                new Farmer
                {
                    Id = 2,
                    FullName = "Bob Fields",
                    Email = "bob@farm.co.za"
                }
            );

            // 2. Seed a few products tied to those farmers
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Sunflower Seeds",
                    Category = "Grains",
                    ProductionDate = new DateTime(2025, 1, 15),
                    FarmerId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Organic Wheat",
                    Category = "Grains",
                    ProductionDate = new DateTime(2025, 2, 10),
                    FarmerId = 2
                }
            );
        }
    }
}
