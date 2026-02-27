using DomainLayer.Models.PlanModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Models.PlanModule
{
    public class PlanBudgetItem
    {
        public int PlanBudgetItemId { get; set; }
        public int PlanId { get; set; }

        public string Category { get; set; } = default!; // EX :  Accommodation, Food
        public decimal Spent { get; set; }
        public decimal Limit { get; set; }
        public string Status { get; set; } = default!; // Over or Under

        //Relationship with Plan
        public Plan Plan { get; set; } = default!;
    }
}
