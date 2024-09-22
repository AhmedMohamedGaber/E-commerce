using Demo_1_Ecommerce;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;
using Demo_1_Ecommerce.ViewModels;

namespace Demo_1_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int TotalCarts { get; set; }
        private readonly ICompositeViewEngine _viewEngine;

        public CartController(IUnitOfWork unitOfWork, ICompositeViewEngine viewEngine)
        {
            _unitOfWork = unitOfWork;
            _viewEngine = viewEngine;
            ShoppingCartVM = new ShoppingCartVM();
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == claim.Value, icludeWord: "Product")
            };

            foreach (var item in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.TotalCarts += (item.Count * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == claim.Value, icludeWord: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetByID(x => x.Id == claim.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            return View("summary", ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(ShoppingCartVM shoppingcartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcartVM.CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == claim.Value, icludeWord: "Product");
            shoppingcartVM.OrderHeader.OrderStatus = SD.Pending;
            shoppingcartVM.OrderHeader.PaymentSatuts = SD.Pending;
            shoppingcartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingcartVM.OrderHeader.ApplicationUserId = claim.Value;

            if (shoppingcartVM.CartList != null)
            {
                foreach (var item in shoppingcartVM.CartList)
                {
                    shoppingcartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
                }

                _unitOfWork.OrderHeader.add(shoppingcartVM.OrderHeader);
                _unitOfWork.complete();
            }

            if (shoppingcartVM.CartList != null)
            {
                foreach (var item in shoppingcartVM.CartList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = item.ProductId,
                        OrderId = shoppingcartVM.OrderHeader.Id,
                        Price = item.Product.Price,
                        Count = item.Count
                    };

                    _unitOfWork.OrderDetail.add(orderDetail);
                    _unitOfWork.complete();
                }
            }

            ShoppingCartVM = shoppingcartVM;

            var domain = "http://localhost:1175/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            if (shoppingcartVM.CartList != null)
            {
                foreach (var item in shoppingcartVM.CartList)
                {
                    var sessionLineOption = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100) + (long)((long)(item.Product.Price * 100) * 0.01),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionLineOption);
                }
            }

            var service = new SessionService();
            Session session = service.Create(options);
            shoppingcartVM.OrderHeader.SessionId = session.Id;
            _unitOfWork.complete();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            // Retrieve order details
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetByID(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            // Update order status if paid
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.updateOrderStates(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.complete();
            }

            // Remove shopping carts related to this order
            List<ShopingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.removeRange(shoppingCarts);
            _unitOfWork.complete();

            // Redirect to the OrderConfirmation route with the id
            return RedirectToRoute(new { controller = "Cart", action = "OrderConfirmation", id = id });
        }



        [HttpPost]
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.complete();
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        [HttpPost]
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            if (cartFromDb.Count <= 1)
            {
                return PartialView("_CartWrapper", GetShoppingCartVM());
            }
            cartFromDb.Count -= 1;
            _unitOfWork.complete();
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            _unitOfWork.ShoppingCart.remove(cartFromDb);
            _unitOfWork.complete();
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        private ShoppingCartVM GetShoppingCartVM()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == userId, icludeWord: "Product"),
                TotalCarts = 0
            };

            foreach (var cart in shoppingCartVM.CartList)
            {
                shoppingCartVM.TotalCarts += (cart.Product.Price * cart.Count);
            }

            return shoppingCartVM;
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var shoppingCartVM = GetShoppingCartVM();
            string cartItemsHtml = RenderViewToString("_CartItems", shoppingCartVM);

            return Json(new
            {
                cartItemsHtml = cartItemsHtml,
                total = shoppingCartVM.TotalCarts,
                itemCount = shoppingCartVM.CartList.Sum(c => c.Count)
            });
        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
