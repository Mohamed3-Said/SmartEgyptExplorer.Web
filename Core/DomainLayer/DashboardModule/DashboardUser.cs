using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DashboardModule
{
    public class DashboardUser
    {
        public int DashboardUserId { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "Owner"; // "Admin" | "Owner"

        // Business Info
        public string BusinessType { get; set; } = default!; // "Restaurant" | "Hotel" | "TourGuide"
        public string Title { get; set; } = default!;        // اسم المطعم/الفندق
        public string City { get; set; } = default!;
        public string Country { get; set; } = "Egypt";
        public string? Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? Category { get; set; }   // للمطاعم: Seafood, FastFood...
        public string? DocumentUrl { get; set; } // الوثيقة المرفوعة

        // TourGuide specific
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Languages { get; set; }
        public string? PhotoUrl { get; set; }
        public string? IdStatus { get; set; }   // "Exist" | "Not Exist"

        // Status
        public string Status { get; set; } = "Pending"; // "Pending"|"Approved"|"Rejected"
        public bool IsHidden { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Forgot Password
        public string? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }

        // Navigation
        public ICollection<OwnerService> Services { get; set; } = new List<OwnerService>();

        // Hotel specific - بيملأها الـ Owner بعد Approval
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public string? Description { get; set; }
        public string? PropertyHighlights { get; set; }
        public string? Images { get; set; }         // JSON array
        public string? HouseRules { get; set; }     // JSON array
        public bool? MetroAccess { get; set; }
        public string? LanguagesSpoken { get; set; }
        public string? BookingUrl { get; set; }
    }
}
