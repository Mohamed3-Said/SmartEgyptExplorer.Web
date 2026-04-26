using Microsoft.AspNetCore.Http;
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
        Task<AIPlanResponseDto> GeneratePlanAsync(TripRequest travelData);

        //Voice translation
        Task<AITranslationResult> TranslateTextAsync(AITextTranslateRequest request);
        Task<AITranslationResult> TranslateAudioAsync(IFormFile audioFile, string src, string tgt);
        Task SubmitCorrectionAsync(AICorrectionRequest request);
        Task<bool> HealthCheckAsync();
    }

}
