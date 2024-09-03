using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_1_Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        public string CompanyName { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public bool Sold { get; set; }
        public bool IsCustomized { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public SubCategory SubCategory { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
