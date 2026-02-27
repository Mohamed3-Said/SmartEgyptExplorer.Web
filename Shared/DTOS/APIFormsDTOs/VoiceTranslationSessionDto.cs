using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class VoiceTranslationSessionDto
    {
        public int SessionId { get; set; } = default!;
        public string TranscribedText { get; set; } = default!;
        public string TranslatedText { get; set; } = default!;
        public string OutputAudioUrl { get; set; } = default!;
        public string Status { get; set; } = default!;
    }

}
