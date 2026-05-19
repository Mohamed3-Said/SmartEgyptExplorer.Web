using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BookingDTOs
{
    public class BookTicketDto
    {
        public int AttractionInfoId { get; set; }  
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; } = default!; // CreditCard | MobileWallet
    }
}
