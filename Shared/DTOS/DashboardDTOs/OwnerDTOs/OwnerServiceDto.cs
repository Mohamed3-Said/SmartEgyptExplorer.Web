using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.DashboardDTOs.OwnerDTOs
{
    public class OwnerServiceDto
    {
        public int OwnerServiceId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHidden { get; set; }
    }
}
