using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class HotelDetailsUpdateDto
    {
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public string? Description { get; set; }
        public string? PropertyHighlights { get; set; }
        public bool? MetroAccess { get; set; }
        public string? BookingUrl { get; set; }
        public List<string> LanguagesSpoken { get; set; } = new();
        public List<string> Images { get; set; } = new();
        public List<string> HouseRules { get; set; } = new();
        public List<string> Availability { get; set; } = new();

        public List<string> MostPopularFacilities { get; set; } = new();
        public string? PopularFacilities { get; set; } // JSON string
    }
}
