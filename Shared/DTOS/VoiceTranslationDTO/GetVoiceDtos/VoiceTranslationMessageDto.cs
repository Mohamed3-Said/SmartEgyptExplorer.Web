using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VoiceTranslationDTO.GetVoiceDtos
{
    public class VoiceTranslationMessageDto
    {
        public int VoiceTranslationMessageId { get; set; }
        public string SourceLanguage { get; set; } = default!;
        public string TargetLanguage { get; set; } = default!;
        public string? TranscribedText { get; set; }
        public string? TranslatedText { get; set; }
        public bool HasAudioOutput { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
