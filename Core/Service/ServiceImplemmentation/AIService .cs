using Microsoft.AspNetCore.Http;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.ServiceImplemmentation
{
    public class AIService : IAIService
    {
        private readonly HttpClient _plannerClient;
        private readonly HttpClient _translatorClient;

        public AIService(IHttpClientFactory httpClientFactory)
        {
            _plannerClient = httpClientFactory.CreateClient("AIPlannerClient");
            _translatorClient = httpClientFactory.CreateClient("AITranslatorClient");
        }

        public async Task<AIPlanResponseDto> GeneratePlanAsync(TripRequest travelData)
        {
            var response = await _plannerClient.PostAsJsonAsync("generate-plan", travelData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AI Engine Error: {error}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = await response.Content.ReadFromJsonAsync<AIPlanResponseDto>(options);

            if (result == null)
                throw new Exception("AI returned null response.");

            return result;
        }


        /// Voice translation :
        public async Task<bool> HealthCheckAsync()
        {
            var response = await _translatorClient.GetAsync("/");
            return response.IsSuccessStatusCode;
        }

        public async Task<AITranslationResult> TranslateTextAsync(AITextTranslateRequest request)
        {
            var response = await _translatorClient.PostAsJsonAsync("translate/text", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AI Translation Error: {error}");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = await response.Content.ReadFromJsonAsync<AITranslationResult>(options);

            return result ?? throw new Exception("AI returned null.");
        }

        public async Task<AITranslationResult> TranslateAudioAsync(
            IFormFile audioFile, string src, string tgt)
        {
            using var content = new MultipartFormDataContent();
            using var stream = audioFile.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                audioFile.ContentType ?? "audio/wav");

            content.Add(fileContent, "audio_file", audioFile.FileName);
            content.Add(new StringContent(src), "src");
            content.Add(new StringContent(tgt), "tgt");

            var response = await _translatorClient.PostAsync("translate/audio", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AI Audio Translation Error: {error}");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = await response.Content.ReadFromJsonAsync<AITranslationResult>(options);

            return result ?? throw new Exception("AI returned null.");
        }

        public async Task SubmitCorrectionAsync(AICorrectionRequest request)
        {
            var response = await _translatorClient.PostAsJsonAsync("translate/correct", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AI Correction Error: {error}");
            }
        }
    }
}