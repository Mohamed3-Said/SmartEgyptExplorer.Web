using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    public class InfoItem
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Address { get; set; } = default!;
        public double? Rating { get; set; }
        public string Phone { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;

        // Navigation
        public InfoCategory Category { get; set; } = default!;
    }

}
