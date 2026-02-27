using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class MenuItemPrice
    {
        public int MenuItemPriceId { get; set; }

        public int MenuItemId { get; set; }
        public string VisitorType { get; set; } = default!;    // Local / Foreigner
        [Precision(18, 2)]
        public decimal Price { get; set; }

        // Navigation
        public MenuItem MenuItem { get; set; } = default!;
    }

}
