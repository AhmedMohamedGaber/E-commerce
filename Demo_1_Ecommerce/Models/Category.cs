using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property for related Products
        public ICollection<Product> Products { get; set; }
    }

}
