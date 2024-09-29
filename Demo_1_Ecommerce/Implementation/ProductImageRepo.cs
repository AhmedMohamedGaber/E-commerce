//using Demo_1_Ecommerce.Data;
//using Demo_1_Ecommerce.Models;
//using Demo_1_Ecommerce.Reposatories;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Demo_1_Ecommerce.Implementation
//{
//    public class ProductImageRepo : GenaricRepo<ProductImage>, IProductImageRepo
//    {
//        private readonly ApplicationDbContext _context;

//        public ProductImageRepo(ApplicationDbContext context) : base(context)
//        {
//            _context = context;
//        }

//        public void Update(ProductImage productImage)
//        {
//            var imageInDB = _context.ProductImages.FirstOrDefault(x => x.ImageId == productImage.ImageId);
//            if (imageInDB != null)
//            {
//                imageInDB.ImageUrl = productImage.ImageUrl;
//                imageInDB.ProductId = productImage.ProductId;
//                imageInDB.IsDefaultImage = productImage.IsDefaultImage;
//            }
//        }

//        public IEnumerable<ProductImage> GetImagesByProductId(int productId)
//        {
//            return _context.ProductImages.Where(pi => pi.ProductId == productId).ToList();
//        }

//        public ProductImage GetDefaultImage(int productId)
//        {
//            return _context.ProductImages.FirstOrDefault(pi => pi.ProductId == productId && pi.IsDefaultImage);
//        }
//    }
//}
