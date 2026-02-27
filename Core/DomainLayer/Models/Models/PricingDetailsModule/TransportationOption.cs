using DomainLayer.Models.PlaceModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class TransportationOption
    {
        public int Id { get; set; }

        public int PlaceId { get; set; }
        public string Method { get; set; } = default!;         // Taxi / Metro / Bus / Walk
        public string Details { get; set; } = default!;
        public int EstimatedTime { get; set; }     // in minutes
        [Precision(18, 2)]
        public decimal EstimatedCost { get; set; }

        // Navigation
        public Place Place { get; set; } = default!;
    }

}
