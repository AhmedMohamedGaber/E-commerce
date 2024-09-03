namespace Demo_1_Ecommerce.ViewModels
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        // Navigation property for users
        public List<UserViewModel> Users { get; set; }
    }
}
