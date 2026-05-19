using DomainLayer.Contracts.Repo.IMobileRepo;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo.MobileRepository
{
    public class TicketRepo : ITicketRepo
    {
        private readonly SmartEgyptDbContext _context;

        public TicketRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(string userId)
       => await _context.Tickets
        .Include(t => t.Attraction)
        .Where(t => t.UserId == userId)
        .OrderByDescending(t => t.CreatedAt)
        .ToListAsync();

        public async Task<Ticket?> GetByIdAsync(int ticketId, string userId)
      => await _context.Tickets
        .Include(t => t.Attraction) 
        .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.UserId == userId);

        public async Task UpdateStatusAsync(int ticketId, string status)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return;
            ticket.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
