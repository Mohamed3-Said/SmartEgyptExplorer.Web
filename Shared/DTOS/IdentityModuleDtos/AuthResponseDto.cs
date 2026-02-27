using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.IdentityModuleDtos
{
    public class AuthResponseDto
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
