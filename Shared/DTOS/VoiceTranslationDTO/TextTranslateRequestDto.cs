using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VoiceTranslationDTO
{
    public class TextTranslateRequestDto
    {
        public string Text { get; set; } = default!;
        public string Src { get; set; } = "ar";
        public string Tgt { get; set; } = "en";
    }

}
