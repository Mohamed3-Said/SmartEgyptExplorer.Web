using DomainLayer.DashboardModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.configurations;
using ServiceAbstraction.Services.DashboardServices;
using Shared.DTOS.DashboardDTOs;
using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IDashboardAuthService _authService;
        private readonly IWebHostEnvironment _env;
        private readonly SmartEgyptDbContext _context;

        public AuthController(IDashboardAuthService authService, IWebHostEnvironment env , SmartEgyptDbContext context)
        {
            _authService = authService;
            _env = env;
            _context = context;
        }
        //----Admin Dashboard Authentication Endpoint----
        [HttpPost("seed-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedAdmin()
        {
            var existing = await _context.DashboardUsers
                .FirstOrDefaultAsync(u => u.Email == "admin@smartegypt.com");
            if (existing != null)
                return BadRequest("Admin already exists.");

            var admin = new DashboardUser
            {
                Email = "admin@smartegypt.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                BusinessType = "None",
                Title = "Admin",
                City = "Cairo",
                Status = "Approved",
                CreatedAt = DateTime.UtcNow
            };

            await _context.DashboardUsers.AddAsync(admin);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Admin created." });
        }

        [HttpPost("register/step1")]
        public async Task<IActionResult> RegisterStep1(
            [FromForm] RegisterStep1Dto dto,
            IFormFile document)
        {
            if (document == null || document.Length == 0)
                return BadRequest("Document is required.");

            // حفظ الـ document
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "documents");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(document.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await document.CopyToAsync(stream);
            var documentUrl = $"/uploads/documents/{fileName}";

            var result = await _authService.RegisterStep1Async(dto, documentUrl);
            return Ok(result);
        }

        [HttpPost("register/step2")]
        public async Task<IActionResult> RegisterStep2([FromBody] RegisterStep2Dto dto)
        {
            await _authService.CompleteRegistrationAsync(dto);
            return Ok(new { message = "Registration complete. Your account is under review." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new { message = "Reset code sent to your email." });
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto dto)
        {
            var valid = await _authService.VerifyCodeAsync(dto);
            if (!valid) return BadRequest(new { message = "Invalid or expired code." });
            return Ok(new { message = "Code verified." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password reset successfully." });
        }

    }
}
