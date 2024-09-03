using System;

namespace Demo_1_Ecommerce.ViewModels
{
    public class WishlistViewModel
    {
        public int WishlistId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // Added for display purposes

        public int UserId { get; set; }
        public string Username { get; set; } // Added for display purposes

        public DateTime CreatedDate { get; set; }
    }
}
