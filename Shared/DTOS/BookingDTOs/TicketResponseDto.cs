using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BookingDTOs
{
    public class TicketResponseDto
    {
        public int TicketId { get; set; }
        public string BookingId { get; set; } = default!;
        public string PlaceName { get; set; } = default!;
        public string? PlaceImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public string QRCode { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
