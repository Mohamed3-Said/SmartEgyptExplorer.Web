using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services;
using Shared.DTOS.VoiceTranslationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/voice")]
    [Authorize]
    public class VoiceTranslationController : ControllerBase
    {
        private readonly IVoiceTranslationService _voiceService;

        public VoiceTranslationController(IVoiceTranslationService voiceService)
        {
            _voiceService = voiceService;
        }

        // Health Check
        [HttpGet("health")]
        [AllowAnonymous]
        public async Task<IActionResult> HealthCheck()
        {
            var isOnline = await _voiceService.HealthCheckAsync();
            return isOnline ? Ok(new { status = "AI Server is online" })
                           : StatusCode(503, new { status = "AI Server is offline" });
        }

        // Start session
        [HttpPost("session/start")]
        public async Task<IActionResult> StartSession()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var sessionId = await _voiceService.StartSessionAsync(userId);
            return Ok(new { sessionId });
        }

        // Translate text
        [HttpPost("session/{sessionId}/translate/text")]
        public async Task<IActionResult> TranslateText(
            int sessionId,
            [FromBody] TextTranslateRequestDto request)
        {
            var result = await _voiceService.TranslateTextAsync(sessionId, request);
            return Ok(result);
        }

        // Translate audio
        [HttpPost("session/{sessionId}/translate/audio")]
        public async Task<IActionResult> TranslateAudio(
            int sessionId,
            IFormFile audioFile,
            [FromForm] string src = "ar",
            [FromForm] string tgt = "en")
        {
            if (audioFile == null || audioFile.Length == 0)
                return BadRequest("Audio file is required");

            var result = await _voiceService.TranslateAudioAsync(sessionId, audioFile, src, tgt);
            return Ok(result);
        }

        // Submit correction
        [HttpPost("correction")]
        public async Task<IActionResult> SubmitCorrection(
            [FromBody] CorrectionRequestDto request)
        {
            await _voiceService.SubmitCorrectionAsync(request);
            return Ok(new { status = "success", message = "Correction saved." });
        }

        // End session
        [HttpPost("session/{sessionId}/end")]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            await _voiceService.EndSessionAsync(sessionId);
            return Ok(new { message = "Session ended successfully." });
        }


        // GET api/voice/session/{sessionId}/messages
        [HttpGet("session/{sessionId}/messages")]
        public async Task<IActionResult> GetSessionMessages(int sessionId)
        {
            var messages = await _voiceService.GetSessionMessagesAsync(sessionId);
            return Ok(messages);
        }

        // GET api/voice/sessions
        [HttpGet("sessions")]
        public async Task<IActionResult> GetUserSessions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var sessions = await _voiceService.GetUserSessionsAsync(userId);
            return Ok(sessions);
        }

        // DELETE api/voice/session/{sessionId}
        [HttpDelete("session/{sessionId}")]
        public async Task<IActionResult> DeleteSession(int sessionId)
        {
            await _voiceService.DeleteSessionAsync(sessionId);
            return Ok(new { message = "Session deleted successfully." });
        }

        // DELETE api/voice/message/{messageId}
        [HttpDelete("message/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            await _voiceService.DeleteMessageAsync(messageId);
            return Ok(new { message = "Message deleted successfully." });
        }


    }

}
