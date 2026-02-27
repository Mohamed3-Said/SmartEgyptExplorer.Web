using DomainLayer.Models.PricingDetailsModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlaceModule
{
    public class Restaurant
    {
        public int RestaurantId { get; set; } // PK & FK → Place

        public string CuisineType { get; set; } = default!;

        // Navigation
        public int PlaceId { get; set; }   // FK ✅
        public Place Place { get; set; } = default!;
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
