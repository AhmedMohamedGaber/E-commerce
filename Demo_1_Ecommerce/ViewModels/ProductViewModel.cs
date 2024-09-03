namespace Demo_1_Ecommerce.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
    }

}
