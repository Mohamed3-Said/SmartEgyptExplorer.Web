using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.Remaining_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Contracts.Repo
{
    public interface IVoiceTranslationRepository
    {
        Task<VoiceTranslationSession> CreateSessionAsync(VoiceTranslationSession session);

        Task<VoiceTranslationSession?> GetActiveSessionAsync(int sessionId);

        Task<VoiceTranslationMessage> AddMessageAsync(VoiceTranslationMessage message);
        Task UpdateMessageAsync(VoiceTranslationMessage message);
        Task EndSessionAsync(int sessionId);
    }
}
