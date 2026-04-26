using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS
{
    public class FoodDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("ingredients")]
        public string Ingredients { get; set; } = string.Empty;

        [JsonPropertyName("instructions")]
        public string Instructions { get; set; } = string.Empty;

        [JsonPropertyName("price_range")]
        public string PriceRange { get; set; } = string.Empty;

        [JsonPropertyName("price_state")]
        public string PriceState { get; set; } = string.Empty;
    }
}
