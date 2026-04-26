using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class VoiceTranslationMessageInputDto
    {
        public int MessageId { get; set; }
        public string TranscribedText { get; set; }
        public string TranslatedText { get; set; }
        public string OutputAudioUrl { get; set; }
    }
}
