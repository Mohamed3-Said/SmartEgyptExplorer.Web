using AutoMapper;
using DomainLayer.Contracts.Repo;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.Remaining_Modules;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation
{
    public class VoiceTranslationService : IVoiceTranslationService
    {
        private readonly IVoiceTranslationRepository _repository;
        private readonly IAIService _aiService;
        private readonly IMapper _mapper;

        public VoiceTranslationService(
            IVoiceTranslationRepository repository,
            IAIService aiService,
            IMapper mapper)
        {
            _repository = repository;
            _aiService = aiService;
            _mapper = mapper;
        }

        // 1️⃣ Start new conversation session
        public async Task<int> StartSessionAsync(string userId)
        {
            var session = new VoiceTranslationSession
            {
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            session = await _repository.CreateSessionAsync(session);
            return session.VoiceTranslationSessionId;
        }

        // 2️⃣ Translate speech inside active session
        public async Task<VoiceTranslationMessageInputDto> TranslateAsync(
            int sessionId,
            string sourceLanguage,
            string targetLanguage,
            string inputAudioUrl)
        {
            // 🔍 Validate session
            var session = await _repository.GetActiveSessionAsync(sessionId);
            if (session == null)
                throw new Exception("Active session not found");

            // 📝 Create message (pending)
            var message = new VoiceTranslationMessage
            {
                VoiceTranslationSessionId = sessionId,
                SourceLanguage = sourceLanguage,
                TargetLanguage = targetLanguage,
                InputAudioUrl = inputAudioUrl,
                CreatedAt = DateTime.UtcNow,
                TranscribedText = "",
                TranslatedText = "",
                OutputAudioUrl = ""
            };

            message = await _repository.AddMessageAsync(message);

            // 🤖 Call AI (STT → Translate → TTS)
            var aiResult = await _aiService.TranslateSpeechAsync(
                new VoiceTranslationRequest
                {
                    SourceLanguage = sourceLanguage,
                    TargetLanguage = targetLanguage,
                    InputAudioUrl = inputAudioUrl
                });

            // 🧾 Update message with results
            message.TranscribedText = aiResult.TranscribedText;
            message.TranslatedText = aiResult.TranslatedText;
            message.OutputAudioUrl = aiResult.OutputAudioUrl;


            // Save update in DB
            await _repository.UpdateMessageAsync(message); 

            // 🔁 Map to DTO
            return _mapper.Map<VoiceTranslationMessageInputDto>(message);
        }

        // 3️⃣ End conversation session
        public async Task EndSessionAsync(int sessionId)
        {
            await _repository.EndSessionAsync(sessionId);
        }
    }

}
