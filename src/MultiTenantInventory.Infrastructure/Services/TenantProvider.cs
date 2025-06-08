using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Domain.Entities;

namespace MultiTenantInventory.Infrastructure.Services
{
    // <summary>
    // Provides the current tenant context for the application.
    // This service is used to retrieve and set the current tenant information.
    // </summary>
    public class TenantProvider : ITenantProvider
    {
        // holds the current tenant instance, null if no tenant is set
        private Tenant? _tenant;

        /// <summary>
        /// Gets the current tenant.
        /// Returns null if no tenant is set.
        /// </summary>
        public Tenant? GetTenant() => _tenant;

        /// <summary>
        /// Sets the current tenant.
        /// The tenant parameter must not be null.
        /// </summary>
        public void SetTenant(Tenant tenant) => _tenant = tenant!;
    }
}
