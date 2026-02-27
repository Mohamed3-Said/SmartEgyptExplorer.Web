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
        [JsonPropertyName("dayNumber")] // اتغيرت من day
        public int DayNumber { get; set; }

        public string Date { get; set; } = default!;

        [JsonPropertyName("mustTryFood")] // اتغيرت من cultural_dish
        public AICulturalDishDto? MustTryFood { get; set; }

        [JsonPropertyName("activities")]
        public List<AIActivityDto> Activities { get; set; } = new();
    }
}
