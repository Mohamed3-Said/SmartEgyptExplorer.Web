using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    using System;

    public class AIResult
    {
        public int AIResultId { get; set; }

        public int UserFormSubmissionId { get; set; }
        public string ResultType { get; set; } = default!;     // Itinerary / Recommendation / Text
        public string ResultJson { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public UserFormSubmission Submission { get; set; } = default!;
    }

}
