using Demo_1_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Demo_1_Ecommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product -> SubCategory relationship to prevent cascade delete
            modelBuilder.Entity<Product>()
         .HasOne(p => p.SubCategory)
         .WithMany(sc => sc.Products)
         .HasForeignKey(p => p.SubCategoryId)
         .OnDelete(DeleteBehavior.Restrict);


            // Similarly, configure other relationships if needed
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);
            // Relationships and constraints can be configured here if needed
        }
    }

}
