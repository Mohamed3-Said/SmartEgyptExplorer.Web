using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class RegisterStep2Dto
    {
        public int DashboardUserId { get; set; }  // من الـ Step 1 response
        public string? Category { get; set; }     // للمطاعم فقط
        public string? Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        // TourGuide specific
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Languages { get; set; }
        public string? PhoneNumber { get; set; }

        // Restaurant specific
        public string? RestaurantImageUrl { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
