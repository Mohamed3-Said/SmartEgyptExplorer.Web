using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using Shared.DTOS.InfoBankDTOs;

namespace Persistence.Data.Repositories.Repo.InfoBankRepository
{
    public class RestaurantRepo : IRestaurantRepo
    {
        private readonly SmartEgyptDbContext _context;

        public RestaurantRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<RestaurantInfo>> GetAllAsync(
            string? city, string? category, int page, int pageSize)
        {
            var query = _context.RestaurantInfos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(r => r.City.ToLower() == city.ToLower());

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(r => r.Category != null &&
                    r.Category.ToLower().Contains(category.ToLower()));

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(r => r.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<RestaurantInfo>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}