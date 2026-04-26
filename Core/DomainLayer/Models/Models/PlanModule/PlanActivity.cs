using DomainLayer.Models.PlaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlanModule
{
    public class PlanActivity
    {
        public int PlanActivityId { get; set; }
        public int PlanDayId { get; set; }
        public int? PlaceId { get; set; }

        public string Title { get; set; } = default!;

        // To The AI :
        public string? Description { get; set; } // لوصف الأكل أو النشاط
        public decimal? Cost { get; set; }        // تكلفة التذكرة أو النشاط
        public decimal? TransportCost { get; set; } // تكلفة المشوار (الـ Ride)

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? MapUrl { get; set; } //optional, in case we want to store a direct link to the location on a map service

        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public string RecommendedTransport { get; set; } = string.Empty;

        public string? ImageURL { get; set; }
        public string Category { get; set; } = string.Empty;
        public PlanDay PlanDay { get; set; } = default!;
        public Place? Place { get; set; }
    }

}
