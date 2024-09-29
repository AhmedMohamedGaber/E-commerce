using System.ComponentModel.DataAnnotations;

namespace Demo_1_Ecommerce.ViewModels
{
    public class Category
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}