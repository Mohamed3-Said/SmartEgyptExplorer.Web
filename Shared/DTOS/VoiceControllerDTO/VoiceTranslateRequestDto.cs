using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VoiceControllerDTO
{
    public class VoiceTranslateRequestDto
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string InputAudioUrl { get; set; }
    }

}
