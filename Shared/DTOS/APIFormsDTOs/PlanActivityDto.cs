using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class PlanActivityDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Cost { get; set; }
        public decimal TransportCost { get; set; }   // ✅ camelCase في الـ output
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? ImageUrl { get; set; }        // ✅ image_url من FastAPI → ImageUrl للـ output
        public string? StartTime { get; set; }
    }
}
