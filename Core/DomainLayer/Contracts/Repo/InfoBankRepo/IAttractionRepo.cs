using DomainLayer.Models.InfoBankModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.InfoBankDTOs;

namespace DomainLayer.Contracts.Repo.InfoBankRepo
{
    public interface IAttractionRepo
    {
        Task<PagedResult<AttractionInfo>> GetAllAsync(
    string? city,
    string? category,
    int page,
    int pageSize);
        Task<AttractionInfo?> GetByIdAsync(int id);
    }
}
