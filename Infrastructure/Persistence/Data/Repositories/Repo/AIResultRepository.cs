using DomainLayer.Contracts.Repo;
using DomainLayer.Models.Remaining_Modules;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class AIResultRepository(SmartEgyptDbContext _context) : IAIResultRepository
    {
        public async Task SaveResultAsync(AIResult result)
        {
            _context.AIResults.Add(result);
            await _context.SaveChangesAsync();
        }
    }
}
