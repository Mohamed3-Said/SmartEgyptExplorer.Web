using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.InfoBankModule;
using DomainLayer.Models.PlaceModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{

    public class Ticket
    {
        public int TicketId { get; set; }
        public string UserId { get; set; } = default!;

        public int AttractionInfoId { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal Total { get; set; }

        public DateTime Date { get; set; }
        public string QRCode { get; set; } = default!;
        public string BookingId { get; set; } = default!;
        public string Status { get; set; } = "Upcoming";
        public string PaymentMethod { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public AppUser User { get; set; } = default!;
        public AttractionInfo Attraction { get; set; } = default!; 
    }

}
