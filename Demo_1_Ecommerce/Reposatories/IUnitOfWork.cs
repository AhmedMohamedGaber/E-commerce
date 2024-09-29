using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IUnitOfWork:IDisposable
    {
        IContactRepo Contact { get; }
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        IShopingCartRepo ShoppingCart { get; }

        IOrderHeaderRepo OrderHeader { get; }
        IOrderDetailRepo OrderDetail { get; }
        IApplicationUserRepo ApplicationUser { get; }
        IReviewRepo Reviews { get; } // Add this line
       // IProductImageRepo ProductImage { get; } // New line for ProductImage repository

        int complete();
    }
}
