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
    public class DashboardOwnerRepo : IDashboardOwnerRepo
    {
        private readonly SmartEgyptDbContext _context;

        public DashboardOwnerRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardUser?> GetOwnerWithServicesAsync(int ownerId)
            => await _context.DashboardUsers
                .Include(u => u.Services.Where(s => !s.IsHidden))
                .FirstOrDefaultAsync(u => u.DashboardUserId == ownerId);

        public async Task<OwnerService?> GetServiceByIdAsync(int serviceId)
            => await _context.OwnerServices
                .FirstOrDefaultAsync(s => s.OwnerServiceId == serviceId);

        public async Task<OwnerService> AddServiceAsync(OwnerService service)
        {
            _context.OwnerServices.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task UpdateServiceAsync(OwnerService service)
        {
            _context.OwnerServices.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(OwnerService service)
        {
            _context.OwnerServices.Remove(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOwnerAsync(DashboardUser owner)
        {
            _context.DashboardUsers.Update(owner);
            await _context.SaveChangesAsync();
        }
    }
}
