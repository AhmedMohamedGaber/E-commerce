using Demo_1_Ecommerce.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Demo_1_Ecommerce.Models;

namespace Demo_1_Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required] // Add validation
        public string Name { get; set; }

        [Required] // Add validation
        public string Description { get; set; }

        public string? img { get; set; } // This will hold the main image URL

        [Required] // Add validation
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

     //   public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>(); // Initialize the collection

        public List<Review>? Reviews { get; set; }
    }
}
