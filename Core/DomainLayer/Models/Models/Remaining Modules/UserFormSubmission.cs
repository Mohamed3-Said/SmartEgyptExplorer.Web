using DomainLayer.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    public class UserFormSubmission
    {
        public int UserFormSubmissionId { get; set; }

        public string UserId { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public AppUser User { get; set; } = default!;
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
        public ICollection<AIResult> AIResults { get; set; } = new List<AIResult>();
    }
}
