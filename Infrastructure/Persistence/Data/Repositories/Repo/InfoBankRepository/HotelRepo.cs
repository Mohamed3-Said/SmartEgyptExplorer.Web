using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo.InfoBankRepository
{
    public class HotelRepo : IHotelRepo
    {
        private readonly SmartEgyptDbContext _context;

        public HotelRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HotelInfo>> GetAllAsync(string? city)
        {
            var query = _context.HotelInfos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(h => h.City.ToLower() == city.ToLower());

            return await query.ToListAsync();
        }

        public async Task<HotelInfo?> GetByIdAsync(int id)
            => await _context.HotelInfos.FindAsync(id);
    }
}
