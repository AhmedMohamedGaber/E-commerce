using System;
using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } // The user's full name

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } // The username for login

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        public string Mobile { get; set; } // The user's mobile number

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } // The user's email address

        public string Address { get; set; } // The user's address

        public string PostCode { get; set; } // Postal code for the address

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } // The user's password

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } // Confirmation password field

        public string ImageUrl { get; set; } // URL to the user's profile picture

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; } // Role assigned to the user (e.g., Admin, Customer)

        public DateTime CreatedDate { get; set; } // The date and time the user was created

        public string RoleName { get; set; } // The role name for display purposes
    }
}
