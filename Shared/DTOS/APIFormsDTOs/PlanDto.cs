using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    // الـ Response النهائي اللي بيطلع للـ Frontend — مطابق لـ FastAPI بالظبط
    public class PlanDto
    {
        public int PlanId { get; set; }
        public string Status { get; set; } = "success";
        public string? City { get; set; }
        public decimal TotalEstimatedCost { get; set; }
        public decimal TotalBudget { get; set; }
        public string? BudgetStatus { get; set; }
        public Dictionary<string, decimal>? RecommendedBudget { get; set; }
        public Dictionary<string, decimal>? ActualSpent { get; set; }
        public List<PlanDayDto> Days { get; set; } = new();

        // ⬇️ دول للـ internal use بس (DB operations) — مش بيطلعوا في الـ response
        // لو محتاجهم في مكان تاني، استخدم DTO منفصل زي PlanSummaryDto
        // internal int PlanId { get; set; }
        internal System.DateTime StartDate { get; set; }
        internal System.DateTime EndDate { get; set; }
    }

}
