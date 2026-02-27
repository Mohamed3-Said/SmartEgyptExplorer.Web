using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction.Services;
using Shared.DTOS.APIFormsDTOs;
using System.Security.Claims;

namespace SmartEgypt.Controllers
{
    [Authorize] // تأمين الـ Controller عشان مفيش حد غريب يدخله
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IFormService _formService;

        public PlansController(IFormService formService)
        {
            _formService = formService;
        }

        // 1. استقبال إجابات الفورم وحفظها
        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitForm([FromBody] IEnumerable<FormAnswerDto> answers)
        {
            // سحب الـ UserId من الـ Token بتاع المستخدم
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Unauthorized();

            var submissionId = await _formService.SubmitFormAsync(userId, answers);

            return Ok(new { SubmissionId = submissionId, Message = "Answers saved successfully!" });
        }

        // 2. طلب توليد الخطة من الـ AI بناءً على الـ SubmissionId
        [HttpPost("generate/{submissionId}")]
        public async Task<IActionResult> GeneratePlan(int submissionId)
        {
            var plan = await _formService.GeneratePlanAsync(submissionId);

            return Ok(plan);
        }

        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var isDeleted = await _formService.DeletePlanAsync(planId, userId);
            if (!isDeleted) return NotFound(new { Message = "Plan not found or you are not authorized." });

            return Ok(new { Message = "Plan deleted successfully!" });
        }
    }
}
