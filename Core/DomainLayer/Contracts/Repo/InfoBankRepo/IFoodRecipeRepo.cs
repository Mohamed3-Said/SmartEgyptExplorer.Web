using DomainLayer.Models.InfoBankModule;
using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.InfoBankRepo
{
    public interface IFoodRecipeRepo
    {
        Task<PagedResult<FoodRecipe>> GetAllAsync(string? category, int page, int pageSize);
        Task<FoodRecipe?> GetByIdAsync(int id);
    }
}
