using DomainLayer.Models.PricingDetailsModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlaceModule
{
    public class Attraction
    {
        public int AttractionId { get; set; } // PK 

        public string AttractionType { get; set; } = default!;
        public decimal Price { get; set; }

        // Navigation
        public int PlaceId { get; set; }      // ✅ Foreign Key
        public Place Place { get; set; } = null!;
        public ICollection<AttractionTicket> AttractionTickets { get; set; } = new List<AttractionTicket>();


    }
}

