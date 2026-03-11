using DomainLayer.Contracts.Repo;
using DomainLayer.Models.IdentityModule;
using Persistence.Data.configurations;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartEgyptDbContext _context; 

        public UserRepository(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetUserProfileAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Plans)           // عشان عدد الخطط
                .Include(u => u.Reviews)         // عشان تقييماته
                .Include(u => u.FormSubmissions) // ده أهم جزء للـ Recommendation
                .ThenInclude(s => s.AIResults) // عشان تجيب آخر نتيجة وصل لها الـ AI
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateProfileAsync(AppUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
