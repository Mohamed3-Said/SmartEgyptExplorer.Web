using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class PlanActivityDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal TransportCost { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string RecommendedTransport { get; set; } = string.Empty;
        // لو حابب تظهر الصورة
        public string? ImageURL { get; set; }
    }
}
