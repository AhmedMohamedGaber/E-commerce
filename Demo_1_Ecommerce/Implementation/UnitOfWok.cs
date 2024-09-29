using Demo_1_Ecommerce;
using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Implementation;
using Demo_1_Ecommerce.Reposatories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Implementation
{
    public class UnitOfWok : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IShopingCartRepo ShoppingCart { get; private set; }
        public IOrderHeaderRepo OrderHeader { get; private set; }
        public IOrderDetailRepo OrderDetail { get; private set; }
        public IApplicationUserRepo ApplicationUser { get; private set; }
        public IReviewRepo Reviews { get; private set; } // Add this line
        public IContactRepo Contact { get; private set; }

      //  public IProductImageRepo ProductImage { get; private set; }
        public UnitOfWok (ApplicationDbContext context) 
        {
            _context = context;
            Category=new CategoryRepo(context);
            Product=new ProductRepo(context);
            ShoppingCart=new ShoppingCartRepo(context);
            OrderHeader=new OredrHeaderRepo(context);   
            OrderDetail=new OredrDetailRepo(context);
            ApplicationUser=new ApplicationUserRepo(context);
            Reviews = new ReviewRepo(_context); // Initialize Reviews here
            Contact=new ContactRepo(context);
          //  ProductImage=new ProductImageRepo(context);

        }


        public int complete()
        {
            return _context.SaveChanges(); // Ensure this matches your context's method
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
