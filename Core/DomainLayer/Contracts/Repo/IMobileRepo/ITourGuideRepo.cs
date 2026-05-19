using DomainLayer.DashboardModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo.IMobileRepo
{
    public interface ITourGuideRepo
    {
        Task<IEnumerable<DashboardUser>> GetApprovedTourGuidesAsync();
    }
}
