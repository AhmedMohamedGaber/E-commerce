using Demo_1_Ecommerce.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface ICategoryRepo : IGenaricRepo<Category> 
    {
        void update(Category category);
    }
}
