namespace Demo_1_Ecommerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public User User { get; set; }
    }

}
