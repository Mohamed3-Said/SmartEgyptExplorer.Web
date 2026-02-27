using DomainLayer.Models.Remaining_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IAIResultRepository
    {
        Task SaveResultAsync(AIResult result);
    }
}
