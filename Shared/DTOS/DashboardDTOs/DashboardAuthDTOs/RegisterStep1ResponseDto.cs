using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.DashboardAuthDTOs
{
    public class RegisterStep1ResponseDto
    {
        public int DashboardUserId { get; set; } // سيتم استخدامه في الخطوة الثانية RegisterStep2Dto
        public string Message { get; set; } = default!;
    }
}
