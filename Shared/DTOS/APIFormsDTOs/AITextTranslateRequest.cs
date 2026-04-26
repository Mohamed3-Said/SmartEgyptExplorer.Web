using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class AITextTranslateRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;

        [JsonPropertyName("src")]
        public string Src { get; set; } = "ar";

        [JsonPropertyName("tgt")]
        public string Tgt { get; set; } = "en";
    }
}
