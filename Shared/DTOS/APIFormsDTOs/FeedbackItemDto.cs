using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class FeedbackItemDto
    {
        public string PlaceId { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}