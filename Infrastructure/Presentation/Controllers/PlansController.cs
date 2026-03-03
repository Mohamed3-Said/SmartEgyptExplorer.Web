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

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentPlan()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var plan = await _formService.GetUserCurrentPlanAsync(userId);

            if (plan == null)
                return NotFound(new { Message = "No current plan found for this user." });

            return Ok(plan);
        }

        // GET: api/plans/history
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var history = await _formService.GetUserHistoryAsync(userId);
            return Ok(history);
        }

        // GET: api/plans/10
        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanDetails(int planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var plan = await _formService.GetPlanDetailsAsync(planId, userId);

            if (plan == null) return NotFound();

            return Ok(plan);
        }
    }
}
