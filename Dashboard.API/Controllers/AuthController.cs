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
    IFormFile document,
    IFormFile? photo) 
        {
            if (document == null || document.Length == 0)
                return BadRequest("Document is required.");

            // =====================
            // حفظ الـ document
            // =====================
            var docFolder = Path.Combine(_env.WebRootPath, "uploads", "documents");
            Directory.CreateDirectory(docFolder);

            var docFileName = $"{Guid.NewGuid()}{Path.GetExtension(document.FileName)}";
            var docPath = Path.Combine(docFolder, docFileName);

            using (var stream = new FileStream(docPath, FileMode.Create))
            {
                await document.CopyToAsync(stream);
            }

            var documentUrl = $"/uploads/documents/{docFileName}";

            // =====================
            //  حفظ الصور TourGuide)
            // =====================
            string? photoUrl = null;

            var normalized = dto.BusinessType.Replace(" ", "");

            if (normalized == "TourGuide" && photo != null && photo.Length > 0)
            {
                var photoFolder = Path.Combine(_env.WebRootPath, "uploads", "photos");
                Directory.CreateDirectory(photoFolder);

                var photoFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var photoPath = Path.Combine(photoFolder, photoFileName);

                using (var stream = new FileStream(photoPath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                photoUrl = $"/uploads/photos/{photoFileName}";
            }

            // =====================
            //  حفظ في DB
            // =====================
            var result = await _authService.RegisterStep1Async(dto, documentUrl, photoUrl);

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
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
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
