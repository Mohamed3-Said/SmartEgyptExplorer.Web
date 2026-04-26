using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS
{
    public class HotelOutputDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "" ;

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("image_url")]
        public List<string> Images { get; set; } = new();

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("metro_access")]
        public string? MetroAccess { get; set; } = string.Empty;

        [JsonPropertyName("reviews")]
        public int Reviews { get; set; }

        [JsonPropertyName("rules")]
        public string? Rules { get; set; } = string.Empty;

    }
}
