using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class VerifyCodeDto
    {
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
