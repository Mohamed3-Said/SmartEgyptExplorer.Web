using DomainLayer.Contracts.Repo.IMobileRepo;
using ServiceAbstraction.Services.IMobileService;
using Shared.DTOS.MobileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation.MobileService
{
    public class TourGuideService : ITourGuideService
    {
        private readonly ITourGuideRepo _repo;

        public TourGuideService(ITourGuideRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TourGuideDto>> GetTourGuidesAsync()
        {
            var guides = await _repo.GetApprovedTourGuidesAsync();

            return guides.Select(g => new TourGuideDto
            {
                DashboardUserId = g.DashboardUserId,
                Name = g.Name,
                Age = g.Age,
                Languages = g.Languages,
                PhoneNumber = g.PhoneNumber,
                PhotoUrl = $"http://smartegyptdashboard.runasp.net{g.PhotoUrl}",
                City = g.City,
                DocumentUrl = $"http://smartegyptdashboard.runasp.net{g.DocumentUrl}"
            });
        }
    }
}
