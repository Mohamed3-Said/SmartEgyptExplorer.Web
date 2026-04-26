using Shared.DTOS.APIFormsDTOs.AIDTOs;
using Shared.DTOS.APIFormsDTOs.AIDTOs.DetailsDTOS;
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
        public string Date { get; set; } = "";
        public string? City { get; set; }
        // 🏨 Hotel
        public HotelOutputDto? Hotel { get; set; }
        // 🍲 Food
        public FoodDto? MustTryFood { get; set; }
        // 🍽️ Meals
        public List<MealDto> Meals { get; set; } = new();
        public List<PlanActivityDto> Activities { get; set; } = new();
    }

}
