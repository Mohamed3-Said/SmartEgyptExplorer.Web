using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    public class UserAnswer
    {
        public int UserAnswerId { get; set; }

        public int UserFormSubmissionId { get; set; }
        public string QuestionKey { get; set; } = default!;    // city, budget, interests...
        public string AnswerValue { get; set; } = default!;

        // Navigation
        public UserFormSubmission Submission { get; set; } = default!;
    }

}
