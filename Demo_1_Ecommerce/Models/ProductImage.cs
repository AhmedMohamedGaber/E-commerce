using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_1_Ecommerce.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public bool DefaultImage { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
