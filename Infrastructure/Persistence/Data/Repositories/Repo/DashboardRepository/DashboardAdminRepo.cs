using DomainLayer.Contracts.Repo.DashboardRepo;
using DomainLayer.DashboardModule;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;

namespace Persistence.Data.Repositories.Repo.DashboardRepository
{
    public class DashboardAdminRepo : IDashboardAdminRepo
    {
        private readonly SmartEgyptDbContext _context;

        public DashboardAdminRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DashboardUser>> GetPendingByTypeAsync(string businessType)
            => await _context.DashboardUsers
                .Where(u => u.BusinessType == businessType && u.Status == "Pending")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<DashboardUser>> GetApprovedByTypeAsync(
            string businessType, string? search, int page, int pageSize)
        {
            var query = _context.DashboardUsers
                .Where(u => u.BusinessType == businessType && u.Status == "Approved");

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u =>
                    u.Title.Contains(search) ||
                    u.City.Contains(search) ||
                    u.Email.Contains(search));

            return await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetApprovedCountAsync(string businessType, string? search)
        {
            var query = _context.DashboardUsers
                .Where(u => u.BusinessType == businessType && u.Status == "Approved");

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u =>
                    u.Title.Contains(search) ||
                    u.City.Contains(search) ||
                    u.Email.Contains(search));

            return await query.CountAsync();
        }

        public async Task<DashboardUser?> GetByIdAsync(int id)
            => await _context.DashboardUsers
                .FirstOrDefaultAsync(u => u.DashboardUserId == id);

        public async Task UpdateAsync(DashboardUser user)
        {
            _context.DashboardUsers.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DashboardUser user)
        {
            _context.DashboardUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveRestaurantAsync(DashboardUser user)
        {
            user.Status = "Approved";
            _context.DashboardUsers.Update(user);

            await _context.RestaurantInfos.AddAsync(new RestaurantInfo
            {
                Name = user.Title,
                Category = user.Category,
                City = user.City,
                Area = user.Location,
                Latitude = user.Latitude,
                Longitude = user.Longitude
            });
            await _context.SaveChangesAsync();
        }

        public async Task ApproveHotelAsync(DashboardUser user)
        {
            user.Status = "Approved";
            _context.DashboardUsers.Update(user);

            await _context.HotelInfos.AddAsync(new HotelInfo
            {
                HotelExternalId = user.DashboardUserId.ToString(),
                Name = user.Title,
                City = user.City,
                Location = user.Location,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Country = user.Country,
                MinPricePerNight = user.MinPricePerNight ?? 0,
                MaxPricePerNight = user.MaxPricePerNight ?? 0,
                Description = user.Description,
                PropertyHighlights = user.PropertyHighlights,
                Images = user.Images,
                HouseRules = user.HouseRules,
                MetroAccess = user.MetroAccess ?? false,
                LanguagesSpoken = user.LanguagesSpoken,
                BookingUrl = user.BookingUrl
            });
            await _context.SaveChangesAsync();
        }
    }
}