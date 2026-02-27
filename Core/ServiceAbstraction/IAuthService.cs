using Shared.DTOS.IdentityModuleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task LogoutAsync(string refreshToken);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);

        //New : 
        Task VerifyResetCodeAsync(VerifyResetCodeDto verifyResetCodeDto);

    }
}
