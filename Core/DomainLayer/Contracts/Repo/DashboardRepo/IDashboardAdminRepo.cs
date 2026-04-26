using DomainLayer.DashboardModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.DashboardRepo
{
    public interface IDashboardAdminRepo
    {
        // Applications (Pending)
        Task<IEnumerable<DashboardUser>> GetPendingByTypeAsync(string businessType);

        // Approved businesses
        Task<IEnumerable<DashboardUser>> GetApprovedByTypeAsync(
            string businessType, string? search, int page, int pageSize);
        Task<int> GetApprovedCountAsync(string businessType, string? search);

        Task<DashboardUser?> GetByIdAsync(int id);
        Task UpdateAsync(DashboardUser user);
        Task DeleteAsync(DashboardUser user);

        Task ApproveRestaurantAsync(DashboardUser user);
        Task ApproveHotelAsync(DashboardUser user);
    }
}

