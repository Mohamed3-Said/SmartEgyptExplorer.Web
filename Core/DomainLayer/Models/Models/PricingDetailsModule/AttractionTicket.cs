using DomainLayer.Models.PlaceModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PricingDetailsModule
{
    public class AttractionTicket
    {
        public int Id { get; set; }
        public int AttractionId { get; set; } //FK 

        public string VisitorType { get; set; } = default!; // مصري، أجنبي، طالب
        public string TicketCategory { get; set; } = "Standard"; // دخول، باقة كاملة، إلخ

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public Attraction Attraction { get; set; } = default!;
    }

}
