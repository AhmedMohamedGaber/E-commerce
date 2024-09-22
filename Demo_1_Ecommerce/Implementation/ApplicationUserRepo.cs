using Demo_1_Ecommerce.Implementation;
using Demo_1_Ecommerce.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Reposatories;

namespace Demo_1_Ecommerce.Implementation
{
    public class ApplicationUserRepo : GenaricRepo<ApplicationUser>,IApplicationUserRepo
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
