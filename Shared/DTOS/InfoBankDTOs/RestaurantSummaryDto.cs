using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.InfoBankDTOs
{
    public class RestaurantSummaryDto
    {
        public int RestaurantInfoId { get; set; }
        public string Name { get; set; } = "";
        public string? Category { get; set; }
        public string City { get; set; } = "";
        public string? Area { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
