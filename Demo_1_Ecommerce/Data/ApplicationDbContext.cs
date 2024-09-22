using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Demo_1_Ecommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ShopingCart> shopingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure to call the base method

            // You can configure relationships and keys here as needed
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId }); // Example composite key, modify as needed

            // Configure any additional properties or relationships
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.OrderId)
                .ValueGeneratedOnAdd(); // If OrderId is identity, ensure it's configured correctly
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<OrderDetail>()
        //        .HasKey(od => od.OrderId); // This should be correctly set up

        //    modelBuilder.Entity<OrderDetail>()
        //        .Property(od => od.OrderId)
        //        .ValueGeneratedOnAdd(); // Ensure it's marked as an identity column
        //    modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        //    {
        //        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        //        // Other configurations if needed
        //    });
        //}
    }
}
