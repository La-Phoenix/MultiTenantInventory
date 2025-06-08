using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, Guid tenantId, string email);
    }
}
