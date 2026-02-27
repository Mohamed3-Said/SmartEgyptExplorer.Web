using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services;
using Shared.DTOS.VoiceControllerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VoiceTranslationController : ControllerBase
    {
        private readonly IVoiceTranslationService _voiceService;

        public VoiceTranslationController(IVoiceTranslationService voiceService)
        {
            _voiceService = voiceService;
        }

        // 1️⃣ Start new session
        [HttpPost("session/start")]
        public async Task<IActionResult> StartSession()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized(); 

            var sessionId = await _voiceService.StartSessionAsync(userId);
            return Ok(new { sessionId });
        }

        // 2️⃣ Translate speech inside session
        [HttpPost("session/{sessionId}/translate")]
        public async Task<IActionResult> Translate(
            int sessionId,
            [FromBody] VoiceTranslateRequestDto request)
        {
            var result = await _voiceService.TranslateAsync(
                sessionId,
                request.SourceLanguage,
                request.TargetLanguage,
                request.InputAudioUrl);

            return Ok(result);
        }

        // 3️⃣ End session
        [HttpPost("session/{sessionId}/end")]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            await _voiceService.EndSessionAsync(sessionId);
            return Ok(new
            {
                statusCode = 200,
                message = "Voice translation session has been ended successfully."
            });
        }
    }

}
