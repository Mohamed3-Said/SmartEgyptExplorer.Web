using DomainLayer.Contracts.Repo;
using DomainLayer.Models.FeedbackModule;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly SmartEgyptDbContext _context;

        public FeedbackRepository(SmartEgyptDbContext context)
        {
            _context = context;
        }

        public async Task SaveFeedbackAsync(List<UserFeedback> feedbacks)
        {
            await _context.UserFeedbacks.AddRangeAsync(feedbacks);
            await _context.SaveChangesAsync();
        }
    }
}
