using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InfoBankDTOs
{
    public class HotelSummaryDto
    {
        public int HotelInfoId { get; set; }
        public string Name { get; set; } = default!;
        public string? Location { get; set; }
        public string City { get; set; } = default!;
        public string? Country { get; set; }
        public decimal MinPricePerNight { get; set; }
        public decimal MaxPricePerNight { get; set; }
        public bool MetroAccess { get; set; }
        public double ReviewScore { get; set; }
        public int NumberOfReviews { get; set; }
        public string? BookingUrl { get; set; }
        public string? FirstImageUrl { get; set; }
    }
}
