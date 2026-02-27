using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlanModule
{
    public class PlanDay
    {
        public int PlanDayId { get; set; }

        public int PlanId { get; set; }
        public int DayNumber { get; set; }   // Day 1, Day 2, Day 3...
        public DateTime Date { get; set; }
        public string? MustTryDishTitle { get; set; }
        public string? MustTryDishDesc { get; set; }
        public string? MustTryFood { get; set; }
        // Navigation
        public Plan Plan { get; set; } = default!;
        public ICollection<PlanActivity> PlanActivities { get; set; } = new List<PlanActivity>();
    }
}
