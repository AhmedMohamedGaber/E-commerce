//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Demo_1_Ecommerce.Models
//{
//    public class ProductImage
//    {
//        [Key]
//        public int ImageId { get; set; }

//        [Required] // Ensure this property is required
//        public string ImageUrl { get; set; }

//        [ForeignKey("Product")]
//        public int ProductId { get; set; }

//        public bool IsDefaultImage { get; set; }

//        public virtual Product Product { get; set; } // Navigation property
//    }
//}
