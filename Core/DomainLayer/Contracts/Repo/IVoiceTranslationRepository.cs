using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.PlanModule;
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

        //Get Sessions :
        Task<IEnumerable<VoiceTranslationMessage>> GetSessionMessagesAsync(int sessionId);
        Task<IEnumerable<VoiceTranslationSession>> GetUserSessionsAsync(string userId);

        //Delete Session & Messages  :
        Task DeleteSessionAsync(int sessionId);
        Task DeleteMessageAsync(int messageId);
    }
}
