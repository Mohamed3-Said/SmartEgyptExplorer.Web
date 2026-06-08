using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS
{
    // ده للاستقبال من FastAPI فقط — image_url فيه string عادي
    public class HotelDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = string.Empty;  // ✅ string مش List

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("metro_access")]
        public string MetroAccess { get; set; } = string.Empty;

        [JsonPropertyName("reviews")]
        public int Reviews { get; set; }

        [JsonPropertyName("rules")]
        public string Rules { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
    }
}
