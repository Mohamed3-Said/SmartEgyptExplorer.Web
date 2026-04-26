using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.InfoBankModule
{
    public class HotelInfo
    {
        public int HotelInfoId { get; set; }
        public string HotelExternalId { get; set; } = default!;   // من hotel_id
        public string Name { get; set; } = default!;
        public string? Location { get; set; }
        public string City { get; set; } = default!;
        public string? Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Pricing
        public decimal MinPricePerNight { get; set; }
        public decimal MaxPricePerNight { get; set; }

        // Details
        public bool MetroAccess { get; set; }
        public double ReviewScore { get; set; }
        public int NumberOfReviews { get; set; }
        public string? Description { get; set; }
        public string? PropertyHighlights { get; set; }
        public string? PopularFacilities { get; set; }

        // Stored as JSON strings
        public string? Images { get; set; }        // JSON array
        public string? Availability { get; set; }  // JSON array
        public string? HouseRules { get; set; }    // JSON array
        public string? LanguagesSpoken { get; set; }

        public string? BookingUrl { get; set; }
    }
}