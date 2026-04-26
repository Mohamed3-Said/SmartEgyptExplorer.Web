using DomainLayer.Contracts.Repo;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class HotelRepository : IHotelRepository
    {
        private readonly SmartEgyptDbContext _context;

        public HotelRepository(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetRandomHotelIdAsync(string city)
        {
            var hotelId = await _context.Hotels
                .Where(h => h.Place.City == city)
                .Select(h => h.HotelId)
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (hotelId == 0) // ❗ مفيش hotel في المدينة
            {
                hotelId = await _context.Hotels
                    .Select(h => h.HotelId)
                    .FirstOrDefaultAsync();
            }

            return hotelId == 0 ? null : hotelId;
        }
    }
}
