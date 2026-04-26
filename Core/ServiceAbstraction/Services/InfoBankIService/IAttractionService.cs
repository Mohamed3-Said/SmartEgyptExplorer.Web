using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.InfoBankIService
{
    public interface IAttractionService
    {
        Task<PagedResult<AttractionSummaryDto>> GetAllAsync(string? city,
    string? category,
    int page,
    int pageSize);
        Task<AttractionDto?> GetByIdAsync(int id);
    }
}
