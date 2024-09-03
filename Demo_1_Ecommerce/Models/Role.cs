using Microsoft.AspNetCore.Identity;

namespace Demo_1_Ecommerce.Models
{
    public class Role : IdentityRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }

}
