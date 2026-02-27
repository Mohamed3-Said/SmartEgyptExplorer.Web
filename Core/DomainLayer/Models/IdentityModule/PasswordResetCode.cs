using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.IdentityModule
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;     // FK to AppUser.Id
        public AppUser User { get; set; } = default!;      // navigation 
        public string Code { get; set; } = default!;       // e.g. "123456"
        public DateTime ExpiresAt { get; set; }            // UTC
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
