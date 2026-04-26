using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo.InfoBankRepository
{
    public class AttractionRepo : IAttractionRepo
    {
        private readonly SmartEgyptDbContext _context;

        public AttractionRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AttractionInfo>> GetAllAsync(
              string? city,
              string? category,
              int page,
              int pageSize)
        {
            var query = _context.AttractionInfos.AsNoTracking().AsQueryable();

            // Filter by city
            if (!string.IsNullOrWhiteSpace(city))
            {
                city = city.ToLower();

                query = query.Where(a =>
                    (a.NormalizedCity != null && a.NormalizedCity.ToLower() == city) ||
                    a.City.ToLower() == city);
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                category = category.ToLower();

                query = query.Where(a =>
                    a.Category != null &&
                    a.Category.ToLower().Contains(category));
            }

            // Clean Data
            query = query.Where(a =>
                !string.IsNullOrEmpty(a.Name) &&
                !string.IsNullOrEmpty(a.ImageUrl) &&
                a.Latitude != 0 &&
                a.Longitude != 0 &&
                a.ExploreDurationMin != null
            );

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(a => a.AverageRating);

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AttractionInfo>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };
        }

        public async Task<AttractionInfo?> GetByIdAsync(int id)
        {
            return await _context.AttractionInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AttractionInfoId == id);
        }

    }
}
