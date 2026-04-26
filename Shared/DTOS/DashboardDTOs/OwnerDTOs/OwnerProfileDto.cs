using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.OwnerDTOs
{
    public class OwnerProfileDto
    {
        public int DashboardUserId { get; set; }
        public string Email { get; set; } = default!;
        public string BusinessType { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Location { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? Category { get; set; }
        public string Status { get; set; } = default!;
        // TourGuide
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Languages { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
