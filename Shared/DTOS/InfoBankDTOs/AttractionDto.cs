using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// AttractionDto.cs
namespace Shared.DTOS.InfoBankDTOs
{
    public class AttractionDto
    {
        public int AttractionInfoId { get; set; }
        public string PlaceId { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Category { get; set; }
        public string City { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public int? ExploreDurationMin { get; set; }
        public string? ImageUrl { get; set; }
        public string? OpeningDays { get; set; }
        public string? SummerOpeningHours { get; set; }
        public string? WinterOpeningHours { get; set; }
        public string? RamadanOpeningHours { get; set; }
        public decimal ForeignerAdultPrice { get; set; }
        public decimal ForeignerStudentPrice { get; set; }
        public decimal ArabAdultPrice { get; set; }
        public decimal ArabStudentPrice { get; set; }
        public decimal EgyptianAdultPrice { get; set; }
        public decimal EgyptianStudentPrice { get; set; }
        public decimal GarageCarPrice { get; set; }
        public decimal GarageBusPrice { get; set; }
        public string? FreeEntryPolicy { get; set; }
        public string? InclusiveTicketAccess { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}
