using Microsoft.AspNetCore.Http;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.VoiceTranslationDTO;
using Shared.DTOS.VoiceTranslationDTO.GetVoiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services
{
    public interface IVoiceTranslationService
    {
        Task<int> StartSessionAsync(string userId);
        Task<VoiceTranslationResponseDto> TranslateTextAsync(int sessionId, TextTranslateRequestDto request);
        Task<VoiceTranslationResponseDto> TranslateAudioAsync(int sessionId, IFormFile audioFile, string src, string tgt);
        Task SubmitCorrectionAsync(CorrectionRequestDto request);
        Task EndSessionAsync(int sessionId);
        Task<bool> HealthCheckAsync();

        //Get Session & Message :
        Task<IEnumerable<VoiceTranslationMessageDto>> GetSessionMessagesAsync(int sessionId);
        Task<IEnumerable<VoiceTranslationSessionGetDto>> GetUserSessionsAsync(string userId);

        //Delete Session & Message :
        Task DeleteSessionAsync(int sessionId);
        Task DeleteMessageAsync(int messageId);
    }
}
