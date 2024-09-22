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
    public class OredrDetailRepo : GenaricRepo<OrderDetail>,IOrderDetailRepo
    {
        private readonly ApplicationDbContext _context;
        public OredrDetailRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }

      

        
    }
}
