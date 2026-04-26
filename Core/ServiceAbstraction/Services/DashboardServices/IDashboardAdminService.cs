using Shared.DTOS.DashboardDTOs;
using Shared.DTOS.DashboardDTOs.Admin_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.DashboardServices
{
    public interface IDashboardAdminService
    {
        // Pending Applications
        Task<IEnumerable<ApplicationRequestDto>> GetPendingAsync(string businessType);
        Task ApproveAsync(int id);
        Task RejectAsync(int id);

        // Approved — CRUD
        Task<PagedResultDto<ApprovedBusinessDto>> GetApprovedAsync(
            string businessType, string? search, int page, int pageSize);
        Task HideAsync(int id);
        Task DeleteAsync(int id);
    }
}
