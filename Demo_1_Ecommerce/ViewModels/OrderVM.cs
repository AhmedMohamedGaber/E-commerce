using Demo_1_Ecommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
