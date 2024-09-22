using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class AdminHomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminHomeController(ApplicationDbContext applicationDb)
        {
            _context = applicationDb;
        }

        public IActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = _context.applicationUsers.Count(),
                TotalOrders = _context.OrderHeaders.Count(),
                TotalProducts = _context.Products.Count(),
                RecentActivities = GetRecentActivities().ToList(),
                UserGrowth = CalculateUserGrowth(),
                OrderGrowth = CalculateOrderGrowth(),
                ProductGrowth = CalculateProductGrowth(),
                CategriesGrowth = CalculateCategriesGrowth(),
                ReviewGrowth = CalculateReviewsGrowth()
            };

            return View(model);
        }


        private int CalculateUserGrowth()
        {
            int currentCount = _context.applicationUsers.Count();
            int previousCount = 1; // Replace with actual previous count
            return previousCount == 0 ? 0 : (currentCount - previousCount) * 100 / previousCount;
        }

        private int CalculateOrderGrowth()
        {
            int currentCount = _context.OrderHeaders.Count();
            int previousCount = 1; // Replace with actual previous count
            return previousCount == 0 ? 0 : (currentCount - previousCount) * 100 / previousCount;
        }

        private int CalculateProductGrowth()
        {
            int currentCount = _context.Products.Count();
            int previousCount = 4; // Replace with actual previous count
            return previousCount == 0 ? 0 : (currentCount - previousCount) * 100 / previousCount;
        }

        private int CalculateCategriesGrowth()
        {
            int currentCount = _context.categories.Count();
            int previousCount = 2; // Replace with actual previous count
            return previousCount == 0 ? 0 : (currentCount - previousCount) * 100 / previousCount;
        }
        private int CalculateReviewsGrowth()
        {
            int currentCount = _context.Reviews.Count();
            int previousCount = 4; // Replace with actual previous count
            return previousCount == 0 ? 0 : (currentCount - previousCount) * 100 / previousCount;
        }

        private string[] GetRecentActivities()
        {
            return new[]
            {
                "User JohnDoe registered.",
                "Order #123 was placed.",
                "Product 'Organic Apples' added."
            };
        }
    }
}
