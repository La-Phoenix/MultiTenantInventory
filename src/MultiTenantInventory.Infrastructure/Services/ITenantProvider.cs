using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Services
{
    /// <summary>
    /// Returns the current tenant's GUID (set by middleware/key extractor).
    /// </summary>
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
