using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.PlaceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{

    public class Review
    {
        public int ReviewId { get; set; }

        public string UserId { get; set; } = default!;
        public int PlaceId { get; set; }

        public int Rating { get; set; }    // 1 – 5
        public string Comment { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public AppUser User { get; set; } = default!;
        public Place Place { get; set; } = default!;
    }

}
