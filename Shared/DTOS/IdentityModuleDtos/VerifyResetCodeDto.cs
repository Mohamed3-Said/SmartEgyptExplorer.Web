using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.IdentityModuleDtos
{
    public class VerifyResetCodeDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(6, MinimumLength = 6,
            ErrorMessage = "Code must be 6 digits.")]
        public string Code { get; set; } = string.Empty;
    }
}

