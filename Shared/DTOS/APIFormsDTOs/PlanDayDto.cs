using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class PlanDayDto
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }

        public List<PlanActivityDto> Activities { get; set; } = new List<PlanActivityDto>();
    }

}
