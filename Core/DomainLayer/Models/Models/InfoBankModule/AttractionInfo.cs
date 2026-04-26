using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.InfoBankModule
{
    public class AttractionInfo
    {
        public int AttractionInfoId { get; set; }
        public string PlaceId { get; set; } = default!;        // EAS_001
        public string Name { get; set; } = default!;
        public string? Category { get; set; }
        public string City { get; set; } = default!;
        public string? NormalizedCity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public int? ExploreDurationMin { get; set; }
        public string? ImageUrl { get; set; }

        // Opening Hours
        public string? OpeningDays { get; set; }
        public string? SummerOpeningHours { get; set; }
        public string? WinterOpeningHours { get; set; }
        public string? RamadanOpeningHours { get; set; }

        // Prices
        public decimal ForeignerAdultPrice { get; set; }
        public decimal ForeignerStudentPrice { get; set; }
        public decimal ArabAdultPrice { get; set; }
        public decimal ArabStudentPrice { get; set; }
        public decimal EgyptianAdultPrice { get; set; }
        public decimal EgyptianStudentPrice { get; set; }

        // Garage
        public decimal GarageCarPrice { get; set; }
        public decimal GarageBusPrice { get; set; }

        // Policies
        public string? FreeEntryPolicy { get; set; }
        public string? InclusiveTicketAccess { get; set; }

        // Ratings
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }

        // Navigation
        public ICollection<AttractionRating> Ratings { get; set; } = new List<AttractionRating>();
    }
}
