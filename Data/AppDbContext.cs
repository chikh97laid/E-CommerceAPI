using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipping> Shippings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<OrderItem>()
                        .HasIndex(oi => new { oi.OrderId, oi.ItemId })
                        .IsUnique();

            modelBuilder.Entity<Review>()
            .HasIndex(x => new { x.CustomerId, x.ItemId })
            .IsUnique();

            modelBuilder.Entity<Order>().Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(30);

            modelBuilder.Entity<Shipping>().Property(o => o.ShippingStatus).HasConversion<string>().HasMaxLength(30);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }           

        }


    }
}
