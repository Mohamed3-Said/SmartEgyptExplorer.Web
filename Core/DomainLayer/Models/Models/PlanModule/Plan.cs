using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.Models.PlanModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PlanModule
{
    public class Plan
    {

        public int PlanId { get; set; }

        public string UserId { get; set; } = default!;     // FK → ApplicationUser
        public string City { get; set; } = default!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string BudgetLevel { get; set; } = default!;        // Low / Medium / High
        public string PreferredTransport { get; set; } = default!; // Taxi / Metro / Walk

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal TotalEstimatedCost { get; set; }
        public string? BudgetStatus { get; set; } // Over Budget / Within Budget
        public decimal TotalPriceEGP { get; set; } 

        // Related to Budget Breakdown
        public ICollection<PlanBudgetItem> BudgetBreakdown { get; set; } = new List<PlanBudgetItem>();

        // Navigation
        public AppUser User { get; set; } = default!;
        public ICollection<PlanDay> PlanDays { get; set; } = new List<PlanDay>();
    }
}
