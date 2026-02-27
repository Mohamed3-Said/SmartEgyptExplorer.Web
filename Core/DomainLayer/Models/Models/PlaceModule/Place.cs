using DomainLayer.Models.PlanModule;
using DomainLayer.Models.PricingDetailsModule;
using DomainLayer.Models.Remaining_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlaceModule
{
    public class Place
    {
        public int PlaceId { get; set; }

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string City { get; set; } = default!;

        // Hotel / Restaurant / Attraction
        public string Category { get; set; } = default!;

        public string ImageURL { get; set; } = default!;
        public string OpeningHours { get; set; } = default!;
        public double RatingAvg { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Navigation Properties
        public Hotel? Hotel { get; set; }
        public Restaurant? Restaurant { get; set; }
        public Attraction Attraction { get; set; } = null!;
        public ICollection<TransportationOption> TransportationOptions { get; set; } = new List<TransportationOption>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<PlanActivity> PlanActivities { get; set; } = new List<PlanActivity>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
