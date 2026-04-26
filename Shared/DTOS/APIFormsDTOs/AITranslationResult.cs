using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class AITranslationResult
    {
        [JsonPropertyName("source_text")]
        public string SourceText { get; set; } = default!;

        [JsonPropertyName("translated_text")]
        public string TranslatedText { get; set; } = default!;

        [JsonPropertyName("audio_base64")]
        public string AudioBase64 { get; set; } = default!;
    }
}
