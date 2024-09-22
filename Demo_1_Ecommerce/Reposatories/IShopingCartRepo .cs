using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Reposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IShopingCartRepo : IGenaricRepo<ShopingCart> 
    {
         void update(ShopingCart ShopingCart);
        public int IncreaseCount(ShopingCart shopinCart, int Count);
        public int DecreaseCount(ShopingCart shopinCart, int Count);
    }
}
