using AutoMapper;
using DomainLayer.Contracts.Repo.InfoBankRepo;
using ServiceAbstraction.Services.InfoBankIService;
using Shared.DTOS.InfoBankDTOs;

namespace Service.ServiceImplemmentation.InfoBankService
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepo _repo;
        private readonly IMapper _mapper;

        public RestaurantService(IRestaurantRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<RestaurantSummaryDto>> GetAllAsync(
            string? city, string? category, int page, int pageSize)
        {
            var result = await _repo.GetAllAsync(city, category, page, pageSize);

            return new PagedResult<RestaurantSummaryDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = _mapper.Map<List<RestaurantSummaryDto>>(result.Data)
            };
        }
    }
}