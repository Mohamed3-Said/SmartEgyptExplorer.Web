using DomainLayer.Contracts.Repo.IMobileRepo;
using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Models.Remaining_Modules;
using ServiceAbstraction.Services.IMobileService;
using Shared.DTOS.BookingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation.MobileService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _repo;
        private readonly IAttractionRepo _attractionRepo;

        public TicketService(ITicketRepo repo, IAttractionRepo attractionRepo)
        {
            _repo = repo;
            _attractionRepo = attractionRepo;
        }

        public async Task<TicketResponseDto> BookAsync(string userId, BookTicketDto dto)
        {
            // جيب الـ Place عشان تاخد السعر
            var attraction = await _attractionRepo.GetByIdAsync(dto.AttractionInfoId);
            if (attraction == null)
                throw new Exception("Place not found.");

            // حدد السعر بناءً على الـ Foreigner price
            var pricePerTicket = attraction.ForeignerAdultPrice;
            var total = pricePerTicket * dto.Quantity;

            // ولد الـ BookingId والـ QR
            var bookingId = $"SEE-{new Random().Next(100000, 999999)}";
            var qrCode = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(bookingId));

            var ticket = new Ticket
            {
                UserId = userId,
                AttractionInfoId = dto.AttractionInfoId,
                Price = pricePerTicket,
                Quantity = dto.Quantity,
                Total = total,
                Date = dto.Date,
                BookingId = bookingId,
                QRCode = qrCode,
                PaymentMethod = dto.PaymentMethod,
                Status = "Upcoming",
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repo.CreateAsync(ticket);

            return MapToDto(saved, attraction.Name, attraction.ImageUrl);
        }

        public async Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId)
        {
            var tickets = await _repo.GetUserTicketsAsync(userId);

            // ✅ تحديث Status تلقائياً
            var now = DateTime.UtcNow;
            foreach (var t in tickets)
            {
                if (t.Date < now && t.Status == "Upcoming")
                    await _repo.UpdateStatusAsync(t.TicketId, "Completed");
            }

            return tickets.Select(t => MapToDto(
                t,
                t.Attraction?.Name ?? "",
                t.Attraction?.ImageUrl));
        }

        public async Task<TicketResponseDto?> GetTicketByIdAsync(string userId, int ticketId)
        {
            var ticket = await _repo.GetByIdAsync(ticketId, userId);
            if (ticket == null) return null;
            return MapToDto(ticket, ticket.Attraction?.Name ?? "", ticket.Attraction?.ImageUrl);
        }

        private TicketResponseDto MapToDto(Ticket t, string placeName, string? placeImage)
     => new TicketResponseDto
     {
         TicketId = t.TicketId,
         BookingId = t.BookingId,
         PlaceName = placeName,
         PlaceImage = placeImage,
         Price = t.Price,
         Quantity = t.Quantity,
         Total = t.Total,
         Date = t.Date,
         QRCode = t.QRCode,
         Status = t.Status,
         PaymentMethod = t.PaymentMethod,
         CreatedAt = t.CreatedAt
     };
    }
}
