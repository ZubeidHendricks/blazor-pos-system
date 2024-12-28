using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<SecurityAuditLog> SecurityAuditLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Product>()
                .HasIndex(p => p.Barcode)
                .IsUnique();

            builder.Entity<SecurityAuditLog>()
                .HasIndex(log => log.UserId);
        }
    }
}