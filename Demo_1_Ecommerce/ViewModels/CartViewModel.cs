using System;

namespace Demo_1_Ecommerce.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // Added for display purposes

        public int Quantity { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; } // Added for display purposes

        public DateTime CreatedDate { get; set; }
    }
}
