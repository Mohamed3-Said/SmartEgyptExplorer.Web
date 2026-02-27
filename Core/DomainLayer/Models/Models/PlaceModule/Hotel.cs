using DomainLayer.Models.PricingDetailsModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlaceModule
{
    public class Hotel
    {
        public int HotelId { get; set; } // PK & FK → Place

        public int Stars { get; set; }

        // Navigation
        public int PlaceId { get; set; }   // FK ✅
        public Place Place { get; set; } = default!;
        public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
    }
}
