using Microsoft.AspNetCore.Identity;

namespace Demo_1_Ecommerce.Models
{
    public class User : IdentityUser
    {
        //public int UserId {  get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        //public bool isAdmin { get; set; }
        public string RoleId { get; set; } // Change to string
        public DateTime CreatedDate { get; set; }

        public Role Role { get; set; }
        public ICollection<ProductReview> ProductReviews { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }


}
