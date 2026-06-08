using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS
{
    public class MealDto
    {
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Cost { get; set; }
        public string Description { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ImageUrl { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
