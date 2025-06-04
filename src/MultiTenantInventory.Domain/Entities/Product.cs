using MultiTenantInventory.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Domain.Entities
{
    public class Product: IBaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string SKU { get; set; } = default!;
        public int Quantity { get; set; }

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
