using System.Collections.Generic;

namespace Demo_1_Ecommerce.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }

        public int TotalCategries { get; set; }

        public int TotalReviews { get; set; }

        public List<string> RecentActivities { get; set; } = new List<string>();

        public int UserGrowth { get; set; }
        public int OrderGrowth { get; set; }
        public int ProductGrowth { get; set; }

        public int CategriesGrowth { get; set; }

        public int ReviewGrowth { get;set; }
    }
}
