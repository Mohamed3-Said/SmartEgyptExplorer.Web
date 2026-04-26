using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string BusinessType { get; set; } = default!;  // Restaurant | Hotel | TourGuide
        public string Status { get; set; } = default!;        // Pending | Approved | Rejected
    }
}
