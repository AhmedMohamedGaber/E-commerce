using System;
using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.ViewModels
{
    public class OrderViewModel
    {
        public int OrderDetailsId { get; set; }

        public string OrderNo { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // Added for display purposes

        public int Quantity { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; } // Added for display purposes

        public string Status { get; set; }

        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; } // Added for display purposes

        public DateTime OrderDate { get; set; }

        public bool IsCancel { get; set; }
    }
}
