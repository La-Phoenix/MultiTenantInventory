using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(value, out var userId) ? userId : null;
        }
        public static Guid? GetTenantId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("tenantId");
            return Guid.TryParse(value, out var tenantId) ? tenantId : null;
        }
    }
}
