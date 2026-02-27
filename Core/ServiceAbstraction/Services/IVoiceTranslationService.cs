using Shared.DTOS.APIFormsDTOs;
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

        Task<VoiceTranslationMessageInputDto> TranslateAsync(
            int sessionId,
            string sourceLanguage,
            string targetLanguage,
            string inputAudioUrl);

        Task EndSessionAsync(int sessionId);
    }


}
