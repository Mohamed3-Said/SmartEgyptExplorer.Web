using DomainLayer.Contracts.Repo.IMobileRepo;
using DomainLayer.DashboardModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo.MobileRepository
{
    public class TourGuideRepo : ITourGuideRepo
    {
        private readonly SmartEgyptDbContext _context;

        public TourGuideRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DashboardUser>> GetApprovedTourGuidesAsync()
        {
            return await _context.DashboardUsers
                .Where(u =>
                    u.BusinessType.Replace(" ", "") == "TourGuide" &&
                    u.Status == "Approved" &&
                    !u.IsHidden)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }
    }
}
