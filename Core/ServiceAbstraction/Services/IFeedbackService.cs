using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOS.APIFormsDTOs;

namespace ServiceAbstraction.Services
{
    public interface IFeedbackService
    {
        Task<string> SubmitFeedbackAsync(string userId, SubmitFeedbackDto dto);
    }
}
