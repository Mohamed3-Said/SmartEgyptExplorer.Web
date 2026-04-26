using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories
{
    public class PasswordResetCodeRepository(SmartEgyptDbContext _context) : IPasswordResetCodeRepository
    {
        public async Task AddAsync(PasswordResetCode code)
        {
            await _context.Set<PasswordResetCode>().AddAsync(code);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetCode?> GetValidCodeAsync(string email, string code)
        {
            return await _context.PasswordResetCodes
                .Include(p => p.User)
                .FirstOrDefaultAsync(p =>
                    p.User.Email == email &&
                    p.Code == code &&
                    !p.IsUsed &&
                    p.ExpiresAt > DateTime.UtcNow);
       
        }
    }

}
