using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.InfoBankIService
{
    public interface IHotelService
    {
        Task<PagedResult<HotelSummaryDto>> GetHotelsAsync(int page, int pageSize, string? city);
        Task<HotelDetailsDto?> GetByIdAsync(int id);
    }
}
