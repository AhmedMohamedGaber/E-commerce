using Demo_1_Ecommerce.Implementation;
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
    public class CategoryRepo : GenaricRepo<Category>,ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Category category)
        {
            var categoryinDB=_context.categories.FirstOrDefault(x=>x.id==category.id);
            if (categoryinDB != null) { 
            categoryinDB.name = category.name;
            categoryinDB.description = category.description;
            categoryinDB.CreatedTime=DateTime.Now;
            }
        }
    }
}
