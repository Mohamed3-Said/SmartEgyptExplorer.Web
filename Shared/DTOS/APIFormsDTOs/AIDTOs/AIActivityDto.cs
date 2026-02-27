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
        [JsonPropertyName("title")] // اتغيرت من place_name
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")] // اتغيرت من type
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("lat")] // تأكد من وجود دي
        public double Lat { get; set; }

        [JsonPropertyName("lng")] // تأكد من وجود دي
        public double Lng { get; set; }

        [JsonPropertyName("cost")] // اتغيرت من cost_egp
        public decimal Cost { get; set; } 

        [JsonPropertyName("startTime")] // اتغيرت من time_str
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = "11:00";

        [JsonPropertyName("image_url")] // تأكد إن محمود يبعتها بنفس الاسم ده
        public string? ImageURL { get; set; }

        [JsonPropertyName("transport_cost")] // تأكد إن محمود هيبعتها كدة
        public decimal TransportCost { get; set; }
    }
}
