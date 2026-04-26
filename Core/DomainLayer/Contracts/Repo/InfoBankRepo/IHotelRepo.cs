using DomainLayer.Models.InfoBankModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.InfoBankRepo
{
    public interface IHotelRepo
    {
        Task<IEnumerable<HotelInfo>> GetAllAsync(string? city);
        Task<HotelInfo?> GetByIdAsync(int id);
    }
}
