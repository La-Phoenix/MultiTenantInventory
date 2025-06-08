using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Infrastructure.Persistence;

namespace MultiTenantInventory.API.Middleware
{
    /// <summary>
    /// Web middleware to resolve the tenant based on the subdomain in the request host.
    /// </summary>
    public class TenantResolutionMiddleware
    {
        public readonly RequestDelegate _next;
        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Will extract the subdomain from the request host.
        private string? GetSubdomain(string host)
        {
            // Split the host by '.' and return the first part as subdomain
            var parts = host.Split('.');
            return parts.Length > 2 ? parts[0] : null; // e.g. "tenant1" from "tenant1.example.com"
        }

        // Will resolve the tenant based on the subdomain and set it in the ITenantProvider.
        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext, ITenantProvider tenantProvider)
        {
            var host = context.Request.Host.Host; // e.g. "tenant1.example.com"
            var subdomain = GetSubdomain(host);

            if (subdomain == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant subdomain not found.");
                return;
            }

            var tenant = await dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Subdomain == subdomain);

            if (tenant == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Tenant not found.");
                return;
            }

            tenantProvider.SetTenant(tenant);

            await _next(context); // Call the next middleware in the pipeline
        }

    }
}
