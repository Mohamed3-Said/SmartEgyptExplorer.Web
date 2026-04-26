using AutoMapper;
using DomainLayer.Contracts.Repo;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.AspNetCore.Http;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.VoiceTranslationDTO;
using Shared.DTOS.VoiceTranslationDTO.GetVoiceDtos;
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
        public async Task<int> StartSessionAsync(string userId)
        {
            var session = new VoiceTranslationSession
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _repository.CreateSessionAsync(session);
            return session.VoiceTranslationSessionId;
        }
        public async Task<bool> HealthCheckAsync()
        {
            return await _aiService.HealthCheckAsync();
        }

        public async Task<VoiceTranslationResponseDto> TranslateTextAsync(
            int sessionId, TextTranslateRequestDto request)
        {
            var session = await _repository.GetActiveSessionAsync(sessionId);
            if (session == null) throw new Exception("Active session not found");

            // Call AI
            var aiResult = await _aiService.TranslateTextAsync(new AITextTranslateRequest
            {
                Text = request.Text,
                Src = request.Src,
                Tgt = request.Tgt
            });

            // Save message in DB
            var message = new VoiceTranslationMessage
            {
                VoiceTranslationSessionId = sessionId,
                SourceLanguage = request.Src,
                TargetLanguage = request.Tgt,
                InputAudioUrl = "",
                TranscribedText = request.Text,
                TranslatedText = aiResult.TranslatedText,
                OutputAudioUrl = "",
                HasAudioOutput = !string.IsNullOrEmpty(aiResult.AudioBase64),
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddMessageAsync(message);

            return new VoiceTranslationResponseDto
            {
                SourceText = aiResult.SourceText,
                TranslatedText = aiResult.TranslatedText,
                AudioBase64 = aiResult.AudioBase64
            };
        }

        public async Task<VoiceTranslationResponseDto> TranslateAudioAsync(
            int sessionId, IFormFile audioFile, string src, string tgt)
        {
            var session = await _repository.GetActiveSessionAsync(sessionId);
            if (session == null) throw new Exception("Active session not found");

            // Call AI
            var aiResult = await _aiService.TranslateAudioAsync(audioFile, src, tgt);

            // Save message in DB
            var message = new VoiceTranslationMessage
            {
                VoiceTranslationSessionId = sessionId,
                SourceLanguage = src,
                TargetLanguage = tgt,
                InputAudioUrl = audioFile.FileName,
                TranscribedText = aiResult.SourceText,
                TranslatedText = aiResult.TranslatedText,
                OutputAudioUrl = "",
                HasAudioOutput = !string.IsNullOrEmpty(aiResult.AudioBase64),
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddMessageAsync(message);

            return new VoiceTranslationResponseDto
            {
                SourceText = aiResult.SourceText,
                TranslatedText = aiResult.TranslatedText,
                AudioBase64 = aiResult.AudioBase64
            };
        }

        public async Task SubmitCorrectionAsync(CorrectionRequestDto request)
        {
            await _aiService.SubmitCorrectionAsync(new AICorrectionRequest
            {
                Src = request.Src,
                Tgt = request.Tgt,
                OriginalText = request.OriginalText,
                CorrectedText = request.CorrectedText
            });
        }

        public async Task EndSessionAsync(int sessionId)
        {
          await _repository.EndSessionAsync(sessionId);
        }

        //Get Session & Message :

        public async Task<IEnumerable<VoiceTranslationMessageDto>> GetSessionMessagesAsync(int sessionId)
        {
            var messages = await _repository.GetSessionMessagesAsync(sessionId);
            return _mapper.Map<IEnumerable<VoiceTranslationMessageDto>>(messages);
        }

        public async Task<IEnumerable<VoiceTranslationSessionGetDto>> GetUserSessionsAsync(string userId)
        {
            var sessions = await _repository.GetUserSessionsAsync(userId);
            return _mapper.Map<IEnumerable<VoiceTranslationSessionGetDto>>(sessions);

        }

        //Delete Session & Message :
        public async Task DeleteSessionAsync(int sessionId)
        {
            await _repository.DeleteSessionAsync(sessionId);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            await _repository.DeleteMessageAsync(messageId);
        }
    }

}
