using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class AICorrectionRequest
    {
        [JsonPropertyName("src")]
        public string Src { get; set; } = default!;

        [JsonPropertyName("tgt")]
        public string Tgt { get; set; } = default!;

        [JsonPropertyName("original_text")]
        public string OriginalText { get; set; } = default!;

        [JsonPropertyName("corrected_text")]
        public string CorrectedText { get; set; } = default!;
    }
}
