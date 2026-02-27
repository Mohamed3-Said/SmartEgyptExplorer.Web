using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs
{
    public class AIPlanResponseDto
    {
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("totalEstimatedCost")] // اتغيرت من total_estimated_cost_egp
        public decimal TotalEstimatedCost { get; set; }

        [JsonPropertyName("actual_spent")]
        public Dictionary<string, decimal> ActualSpent { get; set; } = new();
        public string City { get; set; } = default!;
       // public string BudgetStatus { get; set; } = default!;
       // public List<BudgetBreakdownDto> BudgetBreakdown { get; set; } = new();

        [JsonPropertyName("recommended_budget")]
        public Dictionary<string, decimal> RecommendedBudget { get; set; } = new();

        [JsonPropertyName("days")] // اتغيرت من itinerary
        public List<AIDayDto> Days { get; set; } = new();
    }
}
