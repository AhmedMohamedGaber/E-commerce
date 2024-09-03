using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property for related Products
        public ICollection<Product> Products { get; set; }

        // Navigation property for the related Category
        public Category Category { get; set; }
    }

}
