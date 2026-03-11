using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.APIFormsDTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Nationality { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PreferredLanguage { get; set; }
        public int TotalPlans { get; set; }
        public int TotalReviews { get; set; } // إضافة عدد التقييمات
      //  public string? TopInterest { get; set; } // مثال: "History", "Nature"
    }
}
