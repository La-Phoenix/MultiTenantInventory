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
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        }
        public static Guid GetTenantId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue("tenantId")!);
        }
    }
}
