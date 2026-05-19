using Shared.DTOS.MobileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.IMobileService
{
    public interface ITourGuideService
    {
        Task<IEnumerable<TourGuideDto>> GetTourGuidesAsync();
    }
}
