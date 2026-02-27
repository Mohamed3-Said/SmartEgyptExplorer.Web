using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.IdentityModuleDtos
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MinLength(6)]
        public string Password { get; set; } = default!;

        [Required]
        public string UserName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        [Phone]
        public string PhoneNumber { get; set; } = default!;

        public string? Nationality { get; set; }
        public string? PreferredLanguage { get; set; }

        [Required]
        public string Role { get; set; } = "Tourist";
    }
}
