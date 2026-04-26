using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using ServiceAbstraction.Services;
using Shared.DTOS.IdentityModuleDtos;
using Shared.DTOS.UserProfileDTOs;
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
    public class AuthController(IAuthService _authService , IUserService _userService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var rseult = await _authService.RegisterAsync(registerDto);
            return Ok(rseult);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> LoginAsync([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthResponseDto>> RefreshTokenAsync([FromBody] RefreshTokenRequestDto refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken.RefreshToken);
            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> LogoutAsync([FromBody] RefreshTokenRequestDto refreshToken)
        {
            await _authService.LogoutAsync(refreshToken.RefreshToken);
            return Ok(new { Message = "User logged out successfully." });
        }

        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordDto forgetPasswordDto)
        {
            await _authService.ForgetPasswordAsync(forgetPasswordDto);
            return Ok(new { message = "If an account with that email exists, a password reset Code has been sent." });
        
        }

        [HttpPost("VerifyResetCode")]
        public async Task<ActionResult> VerifyResetCodeAsync([FromBody] VerifyResetCodeDto verifyResetCodeDto)
        {
            await _authService.VerifyResetCodeAsync(verifyResetCodeDto);
            return Ok(new { message = "Reset code is valid." });
        }


        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            await _authService.ResetPasswordAsync(resetPasswordDto);
            return Ok(new { message = "Password has been reset successfully." });
        }

        //User Profile Endpoints
        // 1. GET: api/Account/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // سحب الـ ID من الـ Claims الموجودة في الـ Token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found in token" });

            var profile = await _userService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(profile);
        }

        // 2. PUT: api/Account/profile-update
        [HttpPut("profile-update")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _userService.UpdateProfileAsync(userId, updateDto);
            return result ? Ok(new { message = "Profile updated with image!" }) : BadRequest();
        }

        // 3. DELETE: api/Account/profile
        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found in token" });

            var result = await _userService.DeleteProfileAsync(userId);

            return result
                ? Ok(new { message = "Profile deleted successfully." })
                : BadRequest(new { message = "Failed to delete profile." });
        }
    }
}
