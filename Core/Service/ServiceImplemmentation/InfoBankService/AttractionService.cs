using AutoMapper;
using DomainLayer.Contracts.Repo.InfoBankRepo;
using ServiceAbstraction.Services.InfoBankIService;
using Shared.DTOS.InfoBankDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation.InfoBankService
{
    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepo _repo;
        private readonly IMapper _mapper;

        public AttractionService(IAttractionRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<AttractionSummaryDto>> GetAllAsync(
            string? city,
            string? category,
            int page,
            int pageSize)
        {
            var result = await _repo.GetAllAsync(city, category, page, pageSize);

            return new PagedResult<AttractionSummaryDto>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = _mapper.Map<List<AttractionSummaryDto>>(result.Data)
            };
        }

        public async Task<AttractionDto?> GetByIdAsync(int id)
        {
            var attraction = await _repo.GetByIdAsync(id);
            return attraction == null ? null : _mapper.Map<AttractionDto>(attraction);
        }
    }
}
