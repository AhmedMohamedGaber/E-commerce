using Demo_1_Ecommerce.Models;
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

        public DbSet<Contact> Contacts { get; set; }


       // public DbSet<ProductImage> ProductImages { get; set; } // Add this line
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure to call the base method

            // Composite Key Configuration
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            // Decimal Property Configurations
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Adjust based on your needs

            modelBuilder.Entity<OrderHeader>()
                .Property(oh => oh.TotalPrice)
                .HasColumnType("decimal(18,2)"); // Adjust based on your needs

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)"); // Adjust based on your needs

            // Identity Column Configuration
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.OrderId)
                .ValueGeneratedOnAdd(); // Ensure it's marked as an identity column
           
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
