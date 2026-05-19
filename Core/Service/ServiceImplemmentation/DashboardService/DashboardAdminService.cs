using DomainLayer.Contracts.Repo.DashboardRepo;
using DomainLayer.Models.InfoBankModule;
using ServiceAbstraction.Services.DashboardServices;
using Shared.DTOS.DashboardDTOs.Admin_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation.DashboardService
{
    public class DashboardAdminService : IDashboardAdminService
    {
        private readonly IDashboardAdminRepo _repo;
        private readonly CsvService _csvService;

        public DashboardAdminService(IDashboardAdminRepo repo , CsvService csvService)
        {
            _repo = repo;
            _csvService = csvService;
        }

        public async Task<IEnumerable<ApplicationRequestDto>> GetPendingAsync(string businessType)
        {
            var users = await _repo.GetPendingByTypeAsync(businessType);
            return users.Select(u => new ApplicationRequestDto
            {
                DashboardUserId = u.DashboardUserId,
                Email = u.Email,
                BusinessType = u.BusinessType,
                Title = u.Title,
                City = u.City,
                Location = u.Location,
                Longitude = u.Longitude,
                Latitude = u.Latitude,
                Category = u.Category,
                DocumentUrl = u.DocumentUrl,
                Name = u.Name,
                Age = u.Age,
                Languages = u.Languages,
                PhoneNumber = u.PhoneNumber,
                IdStatus = u.IdStatus,
                PhotoUrl = u.PhotoUrl,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task ApproveAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            if (user.BusinessType == "Restaurant")
            {
                await _repo.ApproveRestaurantAsync(user);

                _csvService.AppendRestaurant(user); 
            }
            else if (user.BusinessType == "Hotel")
                await _repo.ApproveHotelAsync(user);
            else
            {
                user.Status = "Approved";
                await _repo.UpdateAsync(user);
            }
        }

        public async Task RejectAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            user.Status = "Rejected";
            await _repo.UpdateAsync(user);
        }

        public async Task<PagedResultDto<ApprovedBusinessDto>> GetApprovedAsync(
            string businessType, string? search, int page, int pageSize)
        {
            var data = await _repo.GetApprovedByTypeAsync(businessType, search, page, pageSize);
            var total = await _repo.GetApprovedCountAsync(businessType, search);

            return new PagedResultDto<ApprovedBusinessDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Data = data.Select(u => new ApprovedBusinessDto
                {
                    DashboardUserId = u.DashboardUserId,
                    Email = u.Email,
                    Title = u.Title,
                    City = u.City,
                    Location = u.Location,
                    Longitude = u.Longitude,
                    Latitude = u.Latitude,
                    Category = u.Category,
                    IsHidden = u.IsHidden,
                    DocumentUrl = u.DocumentUrl,
                    Name = u.Name,
                    Languages = u.Languages,
                    Age = u.Age,
                    PhoneNumber = u.PhoneNumber,
                    PhotoUrl = u.PhotoUrl
                })
            };
        }

        public async Task HideAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            user.IsHidden = !user.IsHidden;
            await _repo.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            await _repo.DeleteAsync(user);
        }
    }
}
