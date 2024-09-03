using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Address { get; set; }
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public string RoleName { get; set; }

        public List<ProductReviewViewModel> ProductReviews { get; set; }
        public List<WishlistViewModel> Wishlists { get; set; }
        public List<CartViewModel> Carts { get; set; }
        public List<OrderViewModel> Orders { get; set; }

        // Additional Properties for UI
        public IEnumerable<SelectListItem> Roles { get; set; }

        // Add this property for the Login functionality
        public bool RememberMe { get; set; }
    }
}
