using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;
using Shared.DTOS.DashboardDTOs.OwnerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.DashboardServices
{
    public interface IDashboardOwnerService
    {
        Task<OwnerProfileDto> GetProfileAsync(int ownerId);
        Task<OwnerStatusDto> GetStatusAsync(int ownerId);
        Task<IEnumerable<OwnerServiceDto>> GetServicesAsync(int ownerId);
        Task<OwnerServiceDto> AddServiceAsync(int ownerId, OwnerServiceCreateDto dto, string? imageUrl);
        Task UpdateServiceAsync(int ownerId, int serviceId, OwnerServiceCreateDto dto, string? imageUrl);
        Task DeleteServiceAsync(int ownerId, int serviceId);

        Task UpdatePhotoAsync(int ownerId, string photoUrl);
        Task UpdateHotelDetailsAsync(int userId, HotelDetailsUpdateDto dto);
    }
}
