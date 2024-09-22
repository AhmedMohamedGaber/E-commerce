using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Reposatories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Implementation
{
    public class ProductRepo : GenaricRepo<Product>, IProductRepo
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Product product)
        {
            var productinDB =_context.Products.FirstOrDefault(x=>x.Id==product.Id);
            if (productinDB != null) { 
            productinDB.Name = product.Name;
            productinDB.Description = product.Description;
            productinDB.Price=product.Price;
            productinDB.CategoryId=product.CategoryId;
            productinDB.img = product.img;
            
            
            }
        }
    }
}
