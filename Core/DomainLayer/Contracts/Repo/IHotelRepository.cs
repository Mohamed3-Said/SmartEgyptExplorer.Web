using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IHotelRepository
    {
        Task<int?> GetRandomHotelIdAsync(string city);
    }
}
