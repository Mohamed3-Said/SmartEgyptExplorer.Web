using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class RegisterStep1Dto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MinLength(6)]
        public string Password { get; set; } = default!;
        public string BusinessType { get; set; } = default!;  // Restaurant | Hotel | TourGuide
        public string Title { get; set; } = default!;
        public string City { get; set; } = default!;

        // Document بيتبعت كـ IFormFile في الـ Controller => ID = "Document"
    }
}
