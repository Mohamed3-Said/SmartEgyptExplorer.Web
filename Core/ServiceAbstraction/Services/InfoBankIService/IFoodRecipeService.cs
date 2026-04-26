using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.InfoBankDTOs;

namespace ServiceAbstraction.Services.InfoBankIService
{
    public interface IFoodRecipeService
    {
        Task<PagedResult<FoodRecipeSummaryDto>> GetAllAsync(string? category, int page, int pageSize);
        Task<FoodRecipeDetailsDto?> GetByIdAsync(int id);
    }
}
