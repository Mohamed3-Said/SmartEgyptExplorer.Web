using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs
{
    public class AIActivityDto
    {
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        [JsonPropertyName("transport_cost")]
        public decimal TransportCost { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; } = string.Empty;

        [JsonPropertyName("image_url")]
        public string? ImageURL { get; set; }

        // FastAPI مش بيبعته — بس موجود في بكك عشان متكسرش الـ mapping
        [JsonPropertyName("recommendedTransport")]
        public string? RecommendedTransport { get; set; }
    }
}
