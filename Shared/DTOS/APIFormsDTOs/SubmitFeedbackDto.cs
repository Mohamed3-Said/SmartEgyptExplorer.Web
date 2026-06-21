using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class SubmitFeedbackDto
    {
        public List<FeedbackItemDto> Feedback { get; set; } = new();
    }
}
