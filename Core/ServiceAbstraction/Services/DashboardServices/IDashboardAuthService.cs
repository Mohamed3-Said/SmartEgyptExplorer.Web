using Shared.DTOS.DashboardDTOs.DashboardAuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Services.DashboardServices
{
    public interface IDashboardAuthService
    {
        Task<RegisterStep1ResponseDto> RegisterStep1Async(RegisterStep1Dto dto, string documentUrl);
        Task CompleteRegistrationAsync(RegisterStep2Dto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task ForgotPasswordAsync(string email);
        Task<bool> VerifyCodeAsync(VerifyCodeDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
