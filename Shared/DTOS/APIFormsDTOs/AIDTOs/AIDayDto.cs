using Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs
{
    public class AIDayDto
    {
        [JsonPropertyName("dayNumber")]
        public int DayNumber { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; } = default!;

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("hotel")]
        public HotelDto? Hotel { get; set; }   // ✅ HotelDto للاستقبال

        [JsonPropertyName("mustTryFood")]
        public AICulturalDishDto? MustTryFood { get; set; }

        [JsonPropertyName("meals")]
        public List<AIMealDto> Meals { get; set; } = new();

        [JsonPropertyName("activities")]
        public List<AIActivityDto> Activities { get; set; } = new();
    }
}
