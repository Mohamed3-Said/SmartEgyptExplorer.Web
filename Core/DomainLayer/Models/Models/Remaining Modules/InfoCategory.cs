using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Remaining_Modules
{
    public class InfoCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        // Navigation
        public ICollection<InfoItem> InfoItems { get; set; } = new List<InfoItem>();
    }

}
