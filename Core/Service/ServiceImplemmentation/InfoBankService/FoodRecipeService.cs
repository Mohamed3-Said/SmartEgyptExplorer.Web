using AutoMapper;
using DomainLayer.Contracts.Repo.InfoBankRepo;
using ServiceAbstraction.Services.InfoBankIService;
using Shared.DTOS.InfoBankDTOs;

namespace Service.ServiceImplemmentation.InfoBankService
{
    public class FoodRecipeService : IFoodRecipeService
    {
        private readonly IFoodRecipeRepo _repo;
        private readonly IMapper _mapper;

        public FoodRecipeService(IFoodRecipeRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<FoodRecipeSummaryDto>> GetAllAsync(
            string? category, int page, int pageSize)
        {
            var result = await _repo.GetAllAsync(category, page, pageSize);

            return new PagedResult<FoodRecipeSummaryDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = _mapper.Map<List<FoodRecipeSummaryDto>>(result.Data)
            };
        }

        public async Task<FoodRecipeDetailsDto?> GetByIdAsync(int id)
        {
            var recipe = await _repo.GetByIdAsync(id);
            return recipe == null ? null : _mapper.Map<FoodRecipeDetailsDto>(recipe);
        }
    }
}
