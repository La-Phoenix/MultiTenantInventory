using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Services
{
    public class TenantProvider : ITenantProvider
    {
        // For now, return a fixed GUID or throw if not set.
        // We'll replace this once we write real middleware.
        // In a real application, this would be set by middleware or a key extractor.
        private static readonly Guid _defaultTenantId = Guid.Empty;
        public Guid GetTenantId()
            => _defaultTenantId;
    }
}
