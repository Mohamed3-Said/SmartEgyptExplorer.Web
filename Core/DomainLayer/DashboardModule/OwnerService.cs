
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DashboardModule
{
    public class OwnerService
    {
        public int OwnerServiceId { get; set; }
        public int DashboardUserId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsHidden { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DashboardUser Owner { get; set; } = default!;
    }
}
