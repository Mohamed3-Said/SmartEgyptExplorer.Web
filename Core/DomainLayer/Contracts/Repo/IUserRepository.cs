using DomainLayer.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(AppUser user);
    }
}
