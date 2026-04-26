using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.OwnerDTOs
{
    public class OwnerStatusDto
    {
        public string Status { get; set; } = default!;   // Pending | Approved | Rejected
        public string BusinessType { get; set; } = default!;
        public string Title { get; set; } = default!;
    }
}
