using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VoiceTranslationDTO
{
    public class VoiceTranslationResponseDto
    {
        public string SourceText { get; set; } = default!;
        public string TranslatedText { get; set; } = default!;
        public string AudioBase64 { get; set; } = default!;
    }
}
