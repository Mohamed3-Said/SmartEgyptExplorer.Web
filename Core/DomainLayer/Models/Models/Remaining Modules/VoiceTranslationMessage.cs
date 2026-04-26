using DomainLayer.Models.Remaining_Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Models.Remaining_Modules
{
    public class VoiceTranslationMessage
    {
            [Key]
            public int VoiceTranslationMessageId { get; set; }
            public int VoiceTranslationSessionId { get; set; }

            public string SourceLanguage { get; set; } = null!;
            public string TargetLanguage { get; set; } = null!;
            public string InputAudioUrl { get; set; } = null!;

            public string? TranscribedText { get; set; }
            public string? TranslatedText { get; set; }
            public string? OutputAudioUrl { get; set; }

            public DateTime CreatedAt { get; set; }
            public VoiceTranslationSession Session { get; set; } = null!;

            public bool HasAudioOutput { get; set; }
    }

}
