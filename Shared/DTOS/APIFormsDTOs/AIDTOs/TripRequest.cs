using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs
{
    public class TripRequest
    {
        public string arrivalDateTime { get; set; } = default!;
        public string departureDateTime { get; set; } = default!;
        public string cityArrival { get; set; } = default!;
        public string cityDeparture { get; set; } = default!;
        public string nationality { get; set; } = "Foreigner";
        public int adults { get; set; } = 1;
        public int students { get; set; } = 0;
        public int children_6_12 { get; set; } = 0;
        public int children_under_4 { get; set; } = 0;
        public int special_needs_count { get; set; } = 0;
        public double budgetAmount { get; set; }
        public string budgetType { get; set; } = "Total Trip";
        public string tier { get; set; } = "Standard";
        public string pacing { get; set; } = "Balanced";
        public List<string> cities_to_visit { get; set; } = new();
        public string inter_city_transport { get; set; } = "Uber";
        public string food_type { get; set; } = "Local";
        public bool show_recipe_details { get; set; } = true;
        public List<string> restrictions { get; set; } = new();
        public bool include_food { get; set; } = true;
        public bool guide_needed { get; set; } = false;
        public bool visited_before { get; set; } = false;
        public bool accessibility_needs { get; set; } = false;
        public List<string> interests { get; set; } = new();

       // public string Status { get; set; } = string.Empty;
        //public Dictionary<string, decimal> RecommendedBudget { get; set; } = new();
        //public Dictionary<string, decimal> ActualSpent { get; set; } = new();
    }
}
