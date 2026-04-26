using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InfoBankDTOs
{
    public class HotelDetailsDto
    {
        public int HotelInfoId { get; set; }
        public string Name { get; set; } = default!;
        public string? Location { get; set; }
        public string City { get; set; } = default!;
        public string? Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal MinPricePerNight { get; set; }
        public decimal MaxPricePerNight { get; set; }
        public bool MetroAccess { get; set; }
        public double ReviewScore { get; set; }
        public int NumberOfReviews { get; set; }
        public string? Description { get; set; }
        public string? PropertyHighlights { get; set; }
        public List<string> PopularFacilities { get; set; } = new();
        public List<string> LanguagesSpoken { get; set; } = new();
        public string? BookingUrl { get; set; }
        public List<string> Images { get; set; } = new();
        public List<string> HouseRules { get; set; } = new();    
        public List<string> Availability { get; set; } = new();

       // public string? PopularFacilities { get; set; }
       // public string? LanguagesSpoken { get; set; }
    }
}
