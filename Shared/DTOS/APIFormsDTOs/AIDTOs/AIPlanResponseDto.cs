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

        [JsonPropertyName("totalEstimatedCost")] // اتغيرت من total_estimated_cost_egp
        public decimal TotalEstimatedCost { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("budgetStatus")]
        public string BudgetStatus { get; set; } = string.Empty;
        
         public List<BudgetBreakdownDto> BudgetBreakdown { get; set; } = new();
         
        //[JsonPropertyName("actual_spent")]
        //public Dictionary<string, decimal> ActualSpent { get; set; } = new();

        //[JsonPropertyName("totalBudget")]
        //public decimal TotalBudget { get; set; }

        // [JsonPropertyName("recommended_budget")]
        // public Dictionary<string, decimal> RecommendedBudget { get; set; } = new();


        [JsonPropertyName("recommendedBudget")]
        public Dictionary<string, decimal>? RecommendedBudget { get; set; }

        [JsonPropertyName("actualSpent")]
        public Dictionary<string, decimal>? ActualSpent { get; set; }

        [JsonPropertyName("totalBudget")]
        public double TotalBudget { get; set; }


        [JsonPropertyName("days")] // اتغيرت من itinerary
        public List<AIDayDto> Days { get; set; } = new();
    }
}
