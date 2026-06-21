using DomainLayer.Models.FeedbackModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IFeedbackRepository
    {
        Task SaveFeedbackAsync(List<UserFeedback> feedbacks);
    }
}
