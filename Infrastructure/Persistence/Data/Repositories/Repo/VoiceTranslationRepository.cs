using DomainLayer.Contracts.Repo;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Repositories.Repo
{
    public class VoiceTranslationRepository : IVoiceTranslationRepository
    {
        private readonly SmartEgyptDbContext _context;

        public VoiceTranslationRepository(SmartEgyptDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Create new session
        public async Task<VoiceTranslationSession> CreateSessionAsync(VoiceTranslationSession session)
        {
            _context.VoiceTranslationSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        // 2️⃣ Get active session
        public async Task<VoiceTranslationSession?> GetActiveSessionAsync(int sessionId)
        {
            return await _context.VoiceTranslationSessions
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.VoiceTranslationSessionId == sessionId && s.IsActive);
        }

        // 3️⃣ Add message to session
        public async Task<VoiceTranslationMessage> AddMessageAsync(VoiceTranslationMessage message)
        {
            _context.VoiceTranslationMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        // 4️⃣ End session
        public async Task EndSessionAsync(int sessionId)
        {
            var session = await _context.VoiceTranslationSessions
                .FirstOrDefaultAsync(s => s.VoiceTranslationSessionId == sessionId);

            if (session == null) return;

            session.IsActive = false;
            session.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(VoiceTranslationMessage message)
        {
            _context.VoiceTranslationMessages.Update(message);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<VoiceTranslationMessage>> GetSessionMessagesAsync(int sessionId)
        {
            return await _context.VoiceTranslationMessages
                .Where(m => m.VoiceTranslationSessionId == sessionId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<VoiceTranslationSession>> GetUserSessionsAsync(string userId)
        {
            return await _context.VoiceTranslationSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        // Delete session and  messages :
        public async Task DeleteSessionAsync(int sessionId)
        {
            var session = await _context.VoiceTranslationSessions.FindAsync(sessionId);
            if (session != null)
            {
                _context.VoiceTranslationSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _context.VoiceTranslationMessages.FindAsync(messageId);
            if (message != null)
            {
                _context.VoiceTranslationMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }


    }

}
