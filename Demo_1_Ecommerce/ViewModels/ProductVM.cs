using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.ViewModels;

using Microsoft.AspNetCore.Mvc.Rendering;


namespace Demo_1_Ecommerce.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}