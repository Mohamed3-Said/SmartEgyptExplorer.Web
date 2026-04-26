using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.InfoBankModule
{
    public class FoodRecipe
    {
        public int FoodRecipeId { get; set; }
        public string RecipeExternalId { get; set; } = default!;  // ER_001
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? ServeNotes { get; set; }
        public string? Categories { get; set; }
        public decimal MinPriceEGP { get; set; }
        public decimal MaxPriceEGP { get; set; }
        public string? PriceState { get; set; }
        public string? ImageUrl { get; set; }
    }
}
