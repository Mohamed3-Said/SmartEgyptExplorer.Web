using Newtonsoft.Json;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // غيرنا الـ localhost للينك الـ ngrok بتاع محمود
            // تأكد إن العنوان ينتهي بـ /
            _httpClient.BaseAddress = new Uri("https://proliferous-nontypically-michelina.ngrok-free.dev/");

            // نصيحة: زود الـ Timeout هنا عشان الـ AI بياخد وقت
            _httpClient.Timeout = TimeSpan.FromMinutes(3);
        }

        public async Task<VoiceTranslationResult> TranslateSpeechAsync(VoiceTranslationRequest request)
        {
            return new VoiceTranslationResult
            {
                TranscribedText = "Mock Transcribed Text",
                TranslatedText = "Mock Translated Text",
                OutputAudioUrl = "mock-audio.wav"
            };
        }

        public async Task<AIPlanResponseDto> GeneratePlanAsync(object travelData)
        {
            var response = await _httpClient.PostAsJsonAsync("generate-plan", travelData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AI Engine Error: {error}");
            }

            // هنا السحر: بنحول الـ JSON لـ DTO فوراً
            return await response.Content.ReadFromJsonAsync<AIPlanResponseDto>();
        }
    }
}
