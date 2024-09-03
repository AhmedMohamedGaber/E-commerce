namespace Demo_1_Ecommerce.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
