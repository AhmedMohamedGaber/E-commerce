using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_1_Ecommerce.ViewModels
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please enter a value between 1 and 5")]
        public int Rate { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Comment can't be longer than 500 characters")]
        public string Comment { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
