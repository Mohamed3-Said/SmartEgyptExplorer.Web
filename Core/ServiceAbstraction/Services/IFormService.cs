using Shared.DTOS.APIFormsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services
{
    public interface IFormService
    {
        Task<int> SubmitFormAsync(string userId, IEnumerable<FormAnswerDto> answers);
        Task<PlanDto> GeneratePlanAsync(int submissionId);
    }
}
