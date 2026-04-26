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
    public class HotelService : IHotelService
    {
        private readonly IHotelRepo _repo;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<HotelSummaryDto>> GetHotelsAsync(int page, int pageSize, string? city)
        {
            var data = await _repo.GetAllAsync(city);

            var totalCount = data.Count();

            var pagedData = data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<HotelSummaryDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = _mapper.Map<List<HotelSummaryDto>>(pagedData)
            };
        }

        public async Task<HotelDetailsDto?> GetByIdAsync(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            return hotel == null ? null : _mapper.Map<HotelDetailsDto>(hotel);
        }

    }
}
