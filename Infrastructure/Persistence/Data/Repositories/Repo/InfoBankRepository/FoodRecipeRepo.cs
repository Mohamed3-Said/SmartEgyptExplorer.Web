using DomainLayer.Contracts.Repo.InfoBankRepo;
using DomainLayer.Models.InfoBankModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using Shared.DTOS.InfoBankDTOs;

namespace Persistence.Data.Repositories.Repo.InfoBankRepository
{
    public class FoodRecipeRepo : IFoodRecipeRepo
    {
        private readonly SmartEgyptDbContext _context;

        public FoodRecipeRepo(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FoodRecipe>> GetAllAsync(
            string? category, int page, int pageSize)
        {
            var query = _context.FoodRecipes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(f => f.Categories != null &&
                    f.Categories.ToLower().Contains(category.ToLower()));

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(f => f.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<FoodRecipe>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };
        }

        public async Task<FoodRecipe?> GetByIdAsync(int id)
        {
            return await _context.FoodRecipes
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FoodRecipeId == id);
        }
    }
}
