using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.VoiceTranslationDTO.GetVoiceDtos
{
    public class VoiceTranslationSessionGetDto
    {
        public int VoiceTranslationSessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
