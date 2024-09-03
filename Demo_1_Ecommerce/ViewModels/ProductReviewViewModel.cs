using System;
using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.ViewModels
{
    public class ProductReviewViewModel
    {
        public int ReviewId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // Added for display purposes

        public int UserId { get; set; }
        public string Username { get; set; } // Added for display purposes

        public DateTime CreatedDate { get; set; }
    }
}
