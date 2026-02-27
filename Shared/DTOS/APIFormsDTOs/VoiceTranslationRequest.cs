using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class VoiceTranslationRequest
    {
        public string SourceLanguage { get; set; } = default!;
        public string TargetLanguage { get; set; } = default!;
        public string InputAudioUrl { get; set; }  = default!;
    }
}
