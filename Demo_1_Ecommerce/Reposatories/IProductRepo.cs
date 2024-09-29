using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IProductRepo : IGenaricRepo<Product> 
    {
        void update(Product product);
    }
}
