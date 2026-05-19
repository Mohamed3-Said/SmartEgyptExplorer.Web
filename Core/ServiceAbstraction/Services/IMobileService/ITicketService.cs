using Shared.DTOS.BookingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.IMobileService
{
    public interface ITicketService
    {
        Task<TicketResponseDto> BookAsync(string userId, BookTicketDto dto);
        Task<IEnumerable<TicketResponseDto>> GetMyTicketsAsync(string userId);
        Task<TicketResponseDto?> GetTicketByIdAsync(string userId, int ticketId);
    }
}
