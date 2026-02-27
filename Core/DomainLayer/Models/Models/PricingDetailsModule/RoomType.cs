using DomainLayer.Models.PlaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class RoomType
    {

        public int RoomTypeId { get; set; }

        public int HotelId { get; set; }
        public string Name { get; set; } = default!;        // Single, Double, Suite
        public int Capacity { get; set; }

        // Navigation
        public Hotel Hotel { get; set; } = default!;
        public ICollection<RoomPrice> RoomPrices { get; set; } = new List<RoomPrice>();
    }
}
