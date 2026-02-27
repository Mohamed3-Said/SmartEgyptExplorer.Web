using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services
{
    public interface IAIService
    {
        Task<AIPlanResponseDto> GeneratePlanAsync(object requestData);
        Task<VoiceTranslationResult> TranslateSpeechAsync(VoiceTranslationRequest request);
    }

}
