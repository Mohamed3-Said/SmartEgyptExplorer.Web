using Shared.DTOS.InfoBankDTOs;


namespace ServiceAbstraction.Services.InfoBankIService
{
    public interface IRestaurantService
    {
        Task<PagedResult<RestaurantSummaryDto>> GetAllAsync(
            string? city, string? category, int page, int pageSize);
    }
}