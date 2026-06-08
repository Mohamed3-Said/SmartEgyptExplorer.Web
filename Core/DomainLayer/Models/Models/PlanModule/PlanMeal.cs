using DomainLayer.Models.PlanModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Models.PlanModule
{
    public class PlanMeal
    {
        public int PlanMealId { get; set; }

        public int PlanDayId { get; set; }
        public string Type { get; set; } = default!; // Lunch / Dinner

        public string Name { get; set; } = default!;
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ImageUrl { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

        public PlanDay PlanDay { get; set; } = default!;
    }
}
