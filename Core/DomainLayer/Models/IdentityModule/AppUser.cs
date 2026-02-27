using DomainLayer.Models.PlanModule;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.IdentityModule
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string? Nationality { get; set; } 
        public string? PreferredLanguage { get; set; }  
        public string? ProfileImageUrl { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // # Navigation properties  :
        public ICollection<Plan> Plans { get; set; } = new List<Plan>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<UserFormSubmission> FormSubmissions { get; set; } = new List<UserFormSubmission>();
        public ICollection<VoiceTranslationSession> VoiceTranslationSessions { get; set; } = new List<VoiceTranslationSession>();
    }
}
