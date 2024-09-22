using Demo_1_Ecommerce.Implementation;
using Demo_1_Ecommerce;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData() 
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(icludeWord: "ApplicationUser");
            return Json(new {data= orderHeaders});
        
        }

        public IActionResult Details(int orderid) 
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetByID(u=>u.Id == orderid , icludeWord: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(x => x.OrderId == orderid, icludeWord: "Product")
            };
            return View("Details", orderVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetByID(u => u.Id == OrderVM.OrderHeader.Id);
            orderfromdb.Name = OrderVM.OrderHeader.Name;
            orderfromdb.Phone = OrderVM.OrderHeader.Phone;
            orderfromdb.Address = OrderVM.OrderHeader.Address;
            orderfromdb.City = OrderVM.OrderHeader.City;

            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderfromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.update(orderfromdb);
            _unitOfWork.complete();
            TempData["Type"] = "info";
            TempData["message"] = "Item has Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderid = orderfromdb.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProccess()
        {
            _unitOfWork.OrderHeader.updateOrderStates(OrderVM.OrderHeader.Id, SD.Proccessing, null);
            _unitOfWork.complete();

            TempData["Type"] = "success";
            TempData["message"] = "Order Status has Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShip()
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetByID(u => u.Id == OrderVM.OrderHeader.Id);
            orderfromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            orderfromdb.OrderStatus = SD.Shipped;
            orderfromdb.ShippingDate = DateTime.Now;

            _unitOfWork.OrderHeader.update(orderfromdb);
            _unitOfWork.complete();

            TempData["Type"] = "info";
            TempData["message"] = "Order has Shipped Successfully";
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderfromdb = _unitOfWork.OrderHeader.GetByID(u => u.Id == OrderVM.OrderHeader.Id);
            if (orderfromdb.PaymentSatuts == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfromdb.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(option);

                _unitOfWork.OrderHeader.updateOrderStates(orderfromdb.Id, SD.Cancelled, SD.Refund);
            }
            else
            {
                _unitOfWork.OrderHeader.updateOrderStates(orderfromdb.Id, SD.Cancelled, SD.Cancelled);
            }
            _unitOfWork.complete();
            TempData["Type"] = "info";
            TempData["message"] = "Order has Cancelled Successfully";
            return RedirectToAction("index", "Order", new { orderid = OrderVM.OrderHeader.Id });
        }

    }
}
