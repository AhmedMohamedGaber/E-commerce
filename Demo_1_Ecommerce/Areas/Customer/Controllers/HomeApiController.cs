using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Demo_1_Ecommerce.Models;
using System.Linq;
using System.Collections.Generic;

namespace Demo_1_Ecommerce.Areas.Customer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HomeApiController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;
		public HomeApiController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		// Get Home Page Data
		[HttpGet("Index")]
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
			var averageRatings = allProducts.ToDictionary(
				p => p.Id,
				p => CalculateAverageRating(p.Reviews)
			);

			return Ok(new { viewModel, averageRatings });
		}

		// Contact Form Post
		[HttpPost("Contact")]
		public IActionResult Contact([FromBody] Contact contact)
		{
			if (ModelState.IsValid)
			{
				contact.CreatedAt = DateTime.Now;
				unitOfWork.Contact.add(contact);
				unitOfWork.complete();
				return Ok(new { success = true, message = "Your message has been sent successfully!" });
			}
			return BadRequest(ModelState);
		}

		// Home API with filtering, sorting, and pagination
		[HttpGet("Home")]
		public IActionResult Home(string categoryName, string searchTerm, decimal? minPrice, decimal? maxPrice, string sortBy, int page = 1, int pageSize = 12)
		{
			IEnumerable<Product> products = unitOfWork.Product.GetAll(icludeWord: "Category,Reviews");

			// Filter by category
			if (!string.IsNullOrEmpty(categoryName))
			{
				products = products.Where(p => p.Category.name == categoryName);
			}

			// Search by term
			if (!string.IsNullOrEmpty(searchTerm))
			{
				products = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
											|| (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
			}

			// Filter by price
			if (minPrice.HasValue)
			{
				products = products.Where(p => p.Price >= minPrice.Value);
			}
			if (maxPrice.HasValue)
			{
				products = products.Where(p => p.Price <= maxPrice.Value);
			}

			// Sorting
			products = sortBy switch
			{
				"price_asc" => products.OrderBy(p => p.Price),
				"price_desc" => products.OrderByDescending(p => p.Price),
				"name_asc" => products.OrderBy(p => p.Name),
				"name_desc" => products.OrderByDescending(p => p.Name),
				_ => products.OrderBy(p => p.Id),
			};

			var total = products.Count();
			var totalPages = (int)Math.Ceiling(total / (double)pageSize);
			var result = products.Select(p => new
			{
				Product = p,
				AverageRating = CalculateAverageRating(p.Reviews)
			}).Skip((page - 1) * pageSize)
			 .Take(pageSize)
			 .ToList();

			return Ok(new
			{
				result = result.Select(r => r.Product).ToList(),
				averageRatings = result.ToDictionary(r => r.Product.Id, r => r.AverageRating),
				totalPages,
				currentPage = page,
				currentCategory = categoryName,
				searchTerm,
				minPrice,
				maxPrice,
				sortBy
			});
		}

		// Add Product to Cart
		[HttpPost("AddToCart")]
		[Authorize]
		public IActionResult AddToCart(int productId, int quantity = 1)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var cartItem = unitOfWork.ShoppingCart.GetByID(u => u.applicationUserId == userId && u.ProductId == productId);

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
			return Ok(new { success = true, message = "Product added to cart successfully." });
		}

		// Get Product Details
		[HttpGet("Details/{productId}")]
		public IActionResult Details(int productId)
		{
			var product = unitOfWork.Product.GetByID(p => p.Id == productId, icludeWord: "Category,Reviews");

			if (product == null)
			{
				return NotFound();
			}

			var averageRating = CalculateAverageRating(product.Reviews);
			return Ok(new { product, averageRating });
		}

		// Add Review for Product
		[HttpPost("AddReview")]
		[Authorize]
		public IActionResult AddReview(int productId, int rate, string comment)
		{
			var product = unitOfWork.Product.GetByID(p => p.Id == productId, icludeWord: "Reviews");
			if (product == null)
			{
				return NotFound();
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var existingReview = product.Reviews.FirstOrDefault(r => r.UserId == userId);
			if (existingReview != null)
			{
				return BadRequest(new { error = "You have already reviewed this product." });
			}

			var review = new Review
			{
				ProductId = productId,
				UserId = userId,
				Rate = rate,
				Comment = comment
			};

			product.Reviews.Add(review);
			unitOfWork.complete();

			return Ok(new { success = true, message = "Your review has been added successfully." });
		}

		// Delete Review
		[HttpPost("DeleteReview")]
		[Authorize]
		public IActionResult DeleteReview(int reviewId)
		{
			var review = unitOfWork.Reviews.GetByID(r => r.Id == reviewId);
			if (review == null)
			{
				return NotFound(new { error = "Review not found." });
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (review.UserId != userId)
			{
				return Unauthorized(new { error = "You are not authorized to delete this review." });
			}

			unitOfWork.Reviews.Remove(review);
			unitOfWork.complete();

			return Ok(new { success = true, message = "Your review has been deleted successfully." });
		}

		// Edit Review
		[HttpGet("EditReview/{reviewId}")]
		[Authorize]
		public IActionResult EditReview(int reviewId)
		{
			var review = unitOfWork.Reviews.GetByID(r => r.Id == reviewId);
			if (review == null)
			{
				return NotFound(new { error = "Review not found." });
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (review.UserId != userId)
			{
				return Unauthorized(new { error = "You are not authorized to edit this review." });
			}

			return Ok(review);
		}

		// Update Review
		[HttpPost("EditReview")]
		[Authorize]
		public IActionResult EditReview([FromBody] Review editedReview)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingReview = unitOfWork.Reviews.GetByID(r => r.Id == editedReview.Id);
			if (existingReview == null)
			{
				return NotFound(new { error = "Review not found." });
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (existingReview.UserId != userId)
			{
				return Unauthorized(new { error = "You are not authorized to edit this review." });
			}

			existingReview.Rate = editedReview.Rate;
			existingReview.Comment = editedReview.Comment;

			unitOfWork.complete();
			return Ok(new { success = true, message = "Your review has been updated successfully." });
		}

		// Helper method to calculate average rating
		private double CalculateAverageRating(IEnumerable<Review> reviews)
		{
			if (reviews == null || !reviews.Any())
			{
				return 0;
			}
			return reviews.Average(r => r.Rate);
		}
	}
}
