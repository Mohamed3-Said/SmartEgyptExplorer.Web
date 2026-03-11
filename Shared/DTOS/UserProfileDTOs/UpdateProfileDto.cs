using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.DTOS.UserProfileDTOs
{
    public class UpdateProfileDto
    {
        public string FullName { get; set; } = default!;
        public string? Nationality { get; set; }
        public string? PreferredLanguage { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
