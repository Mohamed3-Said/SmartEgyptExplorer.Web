using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs.AIDTOs
{
    public class BudgetBreakdownDto
    {
        public string Category { get; set; } = default!; // Accommodation, Food, etc.
        public decimal Spent { get; set; }
        public decimal Limit { get; set; }
        public string Status { get; set; } = default!; // Over, Under
    }
}
