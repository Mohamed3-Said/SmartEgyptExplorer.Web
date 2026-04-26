using DomainLayer.DashboardModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.DashboardRepo
{
    public interface IDashboardAuthRepo
    {
        Task<DashboardUser?> GetByEmailAsync(string email);
        Task<DashboardUser?> GetByIdAsync(int id);
        Task<DashboardUser> CreateAsync(DashboardUser user);
        Task UpdateAsync(DashboardUser user);
    }
}
