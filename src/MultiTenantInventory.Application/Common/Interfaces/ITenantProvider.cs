using MultiTenantInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Application.Common.Interfaces
{
    public interface ITenantProvider
    {
        Tenant? GetTenant();
        void SetTenant(Tenant tenant);
    }
}
