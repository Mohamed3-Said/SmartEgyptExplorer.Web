using DomainLayer.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts
{
    public interface IPasswordResetCodeRepository
    {
        Task AddAsync(PasswordResetCode code);
        Task SaveChangesAsync();
        Task<PasswordResetCode?> GetValidCodeAsync(string email, string code);
    }
}
