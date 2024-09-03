using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.Models
{
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public User User { get; set; }
    }

}
