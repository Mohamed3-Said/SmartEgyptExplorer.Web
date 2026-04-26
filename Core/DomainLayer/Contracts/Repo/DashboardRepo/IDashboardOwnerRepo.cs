using DomainLayer.DashboardModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.DashboardRepo
{
    public interface IDashboardOwnerRepo
    {
        Task<DashboardUser?> GetOwnerWithServicesAsync(int ownerId);
        Task<OwnerService?> GetServiceByIdAsync(int serviceId);
        Task<OwnerService> AddServiceAsync(OwnerService service);
        Task UpdateServiceAsync(OwnerService service);
        Task UpdateOwnerAsync(DashboardUser owner);
        Task DeleteServiceAsync(OwnerService service);
    }
}
