using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class RoomPrice
    {
        public int RoomPriceId { get; set; }

        public int RoomTypeId { get; set; }
        public string VisitorType { get; set; } = default!;  // Local / Foreigner
        [Precision(18, 2)]
        public decimal PricePerNight { get; set; }

        // Navigation
        public RoomType RoomType { get; set; } = default!;
    }
}
