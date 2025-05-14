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

            builder.Entity<Product>()
                   .HasOne<Farmer>()
                   .WithMany(f => f.Products)
                   .HasForeignKey(p => p.FarmerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
