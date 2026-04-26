using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.PlaceModule;
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
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }

        public string City { get; set; } = default!; // جديد

        // 🆕 Hotel
        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        // 🆕 Must Try Food (نحسنه بعدين)
        public string? MustTryFoodTitle { get; set; }
        public string? MustTryFoodDescription { get; set; }
        public string? MustTryFoodImage { get; set; }

        public string? MustTryFoodIngredients { get; set; } // 🆕
        public string? MustTryFoodInstructions { get; set; } // 🆕
        public string? MustTryFoodPriceRange { get; set; } // 🆕
        public string? MustTryFoodCategory { get; set; }

        public string? HotelName { get; set; }
        public double? HotelPrice { get; set; }
        public string? HotelImage { get; set; }
        public double? HotelRating { get; set; }

        // ✅ زود دول في PlanDay Entity:
        public string? HotelLocation { get; set; }
        public string? HotelMetroAccess { get; set; }
        public int? HotelReviews { get; set; }
        public string? HotelRules { get; set; }
        public string? MustTryFoodPriceState { get; set; }

        // 🆕 Meals
        public ICollection<PlanMeal> Meals { get; set; } = new List<PlanMeal>();

        // Activities
        public ICollection<PlanActivity> PlanActivities { get; set; } = new List<PlanActivity>();

        public Plan Plan { get; set; } = default!;
    }
}
