using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.InfoBankModule
{
    public class RestaurantInfo
    {
        public int RestaurantInfoId { get; set; }
        public string Name { get; set; } = default!;
        public string? Category { get; set; }
        public string City { get; set; } = default!;
        public string? Area { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
