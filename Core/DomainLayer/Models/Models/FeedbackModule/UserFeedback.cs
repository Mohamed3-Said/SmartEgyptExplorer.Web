using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.FeedbackModule
{
    public class UserFeedback
    {
        public int UserFeedbackId { get; set; }
        public string UserId { get; set; } = default!;
        public string PlaceId { get; set; } = default!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
