using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce;
using Demo_1_Ecommerce.Models;

namespace Demo_1_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(); // Return the Contact view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.Now;
                unitOfWork.Contact.add(contact); // Save contact message
                unitOfWork.complete();

                TempData["Type"] = "success";
                TempData["Message"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact"); // Redirect to the Contact page
            }

            return View(contact); // Return the same view with the model if validation fails
        }





        public IActionResult Index()
        {
            var viewModel = new HomeViewModel()
            {
                FeaturedCategories = unitOfWork.Category.GetAll().Take(6).ToList(),
                TopOffers = unitOfWork.Product.GetAll(p => p.Price < 20000, icludeWord: "Category,Reviews").Take(4).ToList(),
                NewArrivals = unitOfWork.Product.GetAll(icludeWord: "Category,Reviews").OrderByDescending(p => p.Id).Take(8).ToList(),
                PopularProducts = unitOfWork.Product.GetAll(icludeWord: "Category,Reviews").Take(4).ToList()
            };

            
            var allProducts = viewModel.TopOffers.Concat(viewModel.NewArrivals).Concat(viewModel.PopularProducts).Distinct();
            ViewBag.AverageRatings = allProducts.ToDictionary(
                p => p.Id,
                p => CalculateAverageRating(p.Reviews)
            );

            return View(viewModel);
        }

        public IActionResult Home(string categoryName, string searchTerm, decimal? minPrice, decimal? maxPrice, string sortBy, int page = 1, int pageSize = 12)
        {
            IEnumerable<Product> products = unitOfWork.Product.GetAll((p) => true, icludeWord: "Category,Reviews");

            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products.Where(p => p.Category.name == categoryName);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                            || (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            switch (sortBy)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }



            var total = products.Count();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            var result = products.Select(p => new
            {
                Product = p,
                AverageRating = CalculateAverageRating(p.Reviews)
            }).Skip((page - 1) * pageSize)
             .Take(pageSize)
             .ToList();

            ViewBag.AverageRatings = result.ToDictionary(r => r.Product.Id, r => r.AverageRating);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentCategory = categoryName;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            ViewBag.Categories = unitOfWork.Category.GetAll().Select(c => c.name).Distinct().ToList();

            return View(result.Select(r => r.Product).ToList());
        }

        

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            var cartItem = unitOfWork.ShoppingCart.GetByID(
                u => u.applicationUserId == userId && u.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new ShopingCart
                {
                    ProductId = productId,
                    applicationUserId = userId,
                    Count = quantity
                };
                unitOfWork.ShoppingCart.add(cartItem);
            }
            else
            {
                unitOfWork.ShoppingCart.IncreaseCount(cartItem, quantity);
            }

            unitOfWork.complete();

            return Json(new { success = true, message = "Product added to cart successfully." });
        }

        public IActionResult Detalis(int ProductID)
        {
            ShopingCart shopincartvm = new ShopingCart()
            {
                ProductId = ProductID,
                Product = unitOfWork.Product.GetByID(p => p.Id == ProductID, icludeWord: "Category,Reviews"),
                Count = 1,
            };
            return View("Details", shopincartvm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalis(ShopingCart shoppinCart)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppinCart.applicationUserId = claim.Value;
            var cartFromDatabase = unitOfWork.ShoppingCart.GetByID(
                u => u.applicationUserId == claim.Value && u.ProductId == shoppinCart.ProductId);

            if (cartFromDatabase == null)
            {
                unitOfWork.ShoppingCart.add(shoppinCart);
                HttpContext.Session.SetInt32(SD.SessionKey,
                    unitOfWork.ShoppingCart.GetAll(x => x.applicationUserId == claim.Value).ToList().Count()
                );
                unitOfWork.complete();
            }
            else
            {
                unitOfWork.ShoppingCart.IncreaseCount(cartFromDatabase, shoppinCart.Count);
                unitOfWork.complete();
            }
           

            return RedirectToAction("index","Cart");
        }

         public IActionResult Details(int ProductID)
        {
            ShopingCart shopingcartvm = new ShopingCart()
            {
                ProductId = ProductID,
                Product = unitOfWork.Product.GetByID(p => p.Id == ProductID, icludeWord: "Category,Reviews"),
                Count = 1,
            };

            ViewBag.AverageRating = CalculateAverageRating(shopingcartvm.Product.Reviews);

            return View(shopingcartvm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddReview(int id, int rate, string comment)
        {
            var product = unitOfWork.Product.GetByID(p => p.Id == id, icludeWord: "Reviews");
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        
            var existingReview = product.Reviews.FirstOrDefault(r => r.UserId == userId);
            if (existingReview != null)
            {
                TempData["ErrorMessage"] = "You have already reviewed this product.";
                return RedirectToAction("Details", new { ProductID = id });
            }

            var review = new Review
            {
                ProductId = id,
                UserId = userId,
                Rate = rate,
                Comment = comment
            };

            product.Reviews.Add(review);
            unitOfWork.complete();

            TempData["SuccessMessage"] = "Your review has been added successfully.";
            return RedirectToAction("Details", new { ProductID = id });
        }
       
        
        [Authorize]
        [HttpPost]
        public IActionResult DeleteReview(int reviewId)
        {
            var review = unitOfWork.Reviews.GetByID(r => r.Id == reviewId);
            if (review == null)
            {
                TempData["ErrorMessage"] = "Review not found.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (review.UserId != userId)
            {
                TempData["ErrorMessage"] = "You are not authorized to delete this review.";
                return RedirectToAction("Details", new { ProductID = review.ProductId });
            }

            unitOfWork.Reviews.Remove(review);
            unitOfWork.complete();

            TempData["SuccessMessage"] = "Your review has been deleted successfully.";
            return RedirectToAction("Details", new { ProductID = review.ProductId });
        }



        [HttpGet]
        public IActionResult EditReview(int reviewId)
        {
            var review = unitOfWork.Reviews.GetByID(r => r.Id == reviewId);
            if (review == null)
            {
                TempData["ErrorMessage"] = "Review not found.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (review.UserId != userId)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this review.";
                return RedirectToAction("Details", new { ProductID = review.ProductId });
            }

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReview(Review editedReview)
        {
            if (!ModelState.IsValid)
            {
                return View(editedReview);
            }

            var existingReview = unitOfWork.Reviews.GetByID(r => r.Id == editedReview.Id);
            if (existingReview == null)
            {
                TempData["ErrorMessage"] = "Review not found.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingReview.UserId != userId)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this review.";
                return RedirectToAction("Details", new { ProductID = existingReview.ProductId });
            }

            existingReview.Rate = editedReview.Rate;
            existingReview.Comment = editedReview.Comment;

            unitOfWork.complete();

            TempData["SuccessMessage"] = "Your review has been updated successfully.";
            return RedirectToAction("Details", new { ProductID = existingReview.ProductId });
        }



        private double CalculateAverageRating(IEnumerable<Review> reviews)
        {
            if (reviews == null || !reviews.Any())
            {
                return 0;
            }

            return reviews?.Any() == true ? reviews.Average(r => r.Rate) : 0;
        }

        private double CalculateAverageRating(List<Review> reviews)
        {
            if (reviews == null || reviews.Count == 0)
            {
                return 0;
            }

            return reviews.Average(r => r.Rate);
        }

    }
}