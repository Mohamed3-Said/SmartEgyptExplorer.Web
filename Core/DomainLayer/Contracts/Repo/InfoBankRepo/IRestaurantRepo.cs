using DomainLayer.Models.InfoBankModule;
using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.InfoBankRepo
{
    public interface IRestaurantRepo
    {
        Task<PagedResult<RestaurantInfo>> GetAllAsync(string? city, string? category, int page, int pageSize);
    }
}
