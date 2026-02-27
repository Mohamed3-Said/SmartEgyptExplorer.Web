using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    using DomainLayer.Models.IdentityModule;
    using DomainLayer.Models.Models.Remaining_Modules;
    using System;

    public class VoiceTranslationSession
    {
        public int VoiceTranslationSessionId { get; set; }

        public string UserId { get; set; } //only string

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        // Navigation
        public ICollection<VoiceTranslationMessage> Messages { get; set; }
    }

}
