using DomainLayer.Models.Remaining_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.IMobileRepo
{
    public interface ITicketRepo
    {
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(string userId);
        Task<Ticket?> GetByIdAsync(int ticketId, string userId);
        Task UpdateStatusAsync(int ticketId, string status);
    }
}
