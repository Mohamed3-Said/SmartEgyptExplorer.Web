using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.InfoBankModule
{
    public class AttractionRating
    {
        public int AttractionRatingId { get; set; }
        public string UserId { get; set; } = default!;      // User_0001
        public string PlaceId { get; set; } = default!;     // EAS_001
        public double Rating { get; set; }

        // Navigation
        public int AttractionInfoId { get; set; }
        public AttractionInfo Attraction { get; set; } = default!;
    }
}
