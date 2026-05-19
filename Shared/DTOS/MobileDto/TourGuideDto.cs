using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.MobileDto
{
    public class TourGuideDto
    {
        public int DashboardUserId { get; set; }

        public string? Name { get; set; }

        public int? Age { get; set; }

        public string? Languages { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PhotoUrl { get; set; }

        public string? City { get; set; }
        public string? DocumentUrl { get; set; }
    }
}
