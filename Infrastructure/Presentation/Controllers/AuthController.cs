using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOS.IdentityModuleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
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
    }
}
