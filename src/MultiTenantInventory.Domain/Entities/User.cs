using MultiTenantInventory.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Domain.Entities
{
    public class User: IBaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;
        public bool IsAdmin { get; set; } = false; // Indicates if the user is an admin of the tenant

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
