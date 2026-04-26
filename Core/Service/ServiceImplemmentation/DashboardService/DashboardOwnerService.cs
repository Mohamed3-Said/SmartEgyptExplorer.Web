using DomainLayer.Contracts.Repo.DashboardRepo;
using DomainLayer.DashboardModule;
using ServiceAbstraction.Services.DashboardServices;
using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;
using Shared.DTOS.DashboardDTOs.OwnerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation.DashboardService
{
    public class DashboardOwnerService : IDashboardOwnerService
    {
        private readonly IDashboardOwnerRepo _repo;
        private readonly CsvService _csvService;

        public DashboardOwnerService(IDashboardOwnerRepo repo , CsvService csvService)
        {
            _repo = repo;
            _csvService = csvService;
        }

        public async Task<OwnerProfileDto> GetProfileAsync(int ownerId)
        {
            var owner = await _repo.GetOwnerWithServicesAsync(ownerId);
            if (owner == null) throw new Exception("Owner not found.");

            return new OwnerProfileDto
            {
                DashboardUserId = owner.DashboardUserId,
                Email = owner.Email,
                BusinessType = owner.BusinessType,
                Title = owner.Title,
                City = owner.City,
                Location = owner.Location,
                Category = owner.Category,
                Status = owner.Status ,
                // TourGuide
                Name = owner.Name,
                Age = owner.Age,
                Languages = owner.Languages,
                PhotoUrl = owner.PhotoUrl
            };
        }

        public async Task<OwnerStatusDto> GetStatusAsync(int ownerId)
        {
            var owner = await _repo.GetOwnerWithServicesAsync(ownerId);
            if (owner == null) throw new Exception("Owner not found.");

            return new OwnerStatusDto
            {
                Status = owner.Status,
                BusinessType = owner.BusinessType,
                Title = owner.Title
            };
        }

        public async Task<IEnumerable<OwnerServiceDto>> GetServicesAsync(int ownerId)
        {
            var owner = await _repo.GetOwnerWithServicesAsync(ownerId);
            if (owner == null) throw new Exception("Owner not found.");

            return owner.Services.Select(s => new OwnerServiceDto
            {
                OwnerServiceId = s.OwnerServiceId,
                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                ImageUrl = s.ImageUrl,
                IsHidden = s.IsHidden
            });
        }

        public async Task<OwnerServiceDto> AddServiceAsync(
            int ownerId, OwnerServiceCreateDto dto, string? imageUrl)
        {
            var service = new OwnerService
            {
                DashboardUserId = ownerId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repo.AddServiceAsync(service);

            return new OwnerServiceDto
            {
                OwnerServiceId = saved.OwnerServiceId,
                Title = saved.Title,
                Description = saved.Description,
                Price = saved.Price,
                ImageUrl = saved.ImageUrl,
                IsHidden = saved.IsHidden
            };
        }

        public async Task UpdateServiceAsync(
            int ownerId, int serviceId, OwnerServiceCreateDto dto, string? imageUrl)
        {
            var service = await _repo.GetServiceByIdAsync(serviceId);
            if (service == null || service.DashboardUserId != ownerId)
                throw new Exception("Service not found.");

            service.Title = dto.Title;
            service.Description = dto.Description;
            service.Price = dto.Price;
            if (imageUrl != null) service.ImageUrl = imageUrl;

            await _repo.UpdateServiceAsync(service);
        }

        public async Task DeleteServiceAsync(int ownerId, int serviceId)
        {
            var service = await _repo.GetServiceByIdAsync(serviceId);
            if (service == null || service.DashboardUserId != ownerId)
                throw new Exception("Service not found.");

            await _repo.DeleteServiceAsync(service);
        }

        public async Task UpdatePhotoAsync(int ownerId, string photoUrl)
        {
            var owner = await _repo.GetOwnerWithServicesAsync(ownerId);
            if (owner == null) throw new Exception("Owner not found.");

            owner.PhotoUrl = photoUrl;

            await _repo.UpdateOwnerAsync(owner);
        }

        public async Task UpdateHotelDetailsAsync(int userId, HotelDetailsUpdateDto dto)
        {
            var user = await _repo.GetOwnerWithServicesAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.MinPricePerNight = dto.MinPricePerNight;
            user.MaxPricePerNight = dto.MaxPricePerNight;
            user.Description = dto.Description;
            user.PropertyHighlights = dto.PropertyHighlights;
            user.MetroAccess = dto.MetroAccess;
            user.BookingUrl = dto.BookingUrl;

            user.Images = JsonSerializer.Serialize(dto.Images);
            user.HouseRules = JsonSerializer.Serialize(dto.HouseRules);
            user.LanguagesSpoken = JsonSerializer.Serialize(dto.LanguagesSpoken);

            await _repo.UpdateOwnerAsync(user);
            _csvService.AppendHotel(user);
        }
    }
}
