using Demo_1_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_1_Ecommerce.ViewModels
{
	public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		[ForeignKey("OrderHeader")]
		public int OrderId { get; set; }
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }		
		public int Count { get; set; }
		public decimal Price { get; set; }

        public string ProductName => Product?.Name; // Using the null conditional operator
    }
}
