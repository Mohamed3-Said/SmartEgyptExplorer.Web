using DomainLayer.Contracts.Repo.DashboardRepo;
using DomainLayer.DashboardModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo.DashboardRepository
{
    public class DashboardAuthRepo : IDashboardAuthRepo
    {
        private readonly SmartEgyptDbContext _context;

        public DashboardAuthRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardUser?> GetByEmailAsync(string email)
            => await _context.DashboardUsers
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<DashboardUser?> GetByIdAsync(int id)
            => await _context.DashboardUsers
                .Include(u => u.Services)
                .FirstOrDefaultAsync(u => u.DashboardUserId == id);

        public async Task<DashboardUser> CreateAsync(DashboardUser user)
        {
            _context.DashboardUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(DashboardUser user)
        {
            _context.DashboardUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
