using DomainLayer.Models.PlaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        public int RestaurantId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsSpecial { get; set; }

        // Navigation
        public Restaurant Restaurant { get; set; } = default!;
        public ICollection<MenuItemPrice> MenuItemPrices { get; set; } = new List<MenuItemPrice>();
    }

}
