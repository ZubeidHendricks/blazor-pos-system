using Microsoft.EntityFrameworkCore;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            // Add some seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Coffee", Barcode = "123456", Price = 2.50m, Stock = 100, Category = "Beverages" },
                new Product { Id = 2, Name = "Tea", Barcode = "123457", Price = 2.00m, Stock = 100, Category = "Beverages" },
                new Product { Id = 3, Name = "Sandwich", Barcode = "123458", Price = 5.00m, Stock = 50, Category = "Food" },
                new Product { Id = 4, Name = "Cookie", Barcode = "123459", Price = 1.50m, Stock = 200, Category = "Food" }
            );
        }
    }
}