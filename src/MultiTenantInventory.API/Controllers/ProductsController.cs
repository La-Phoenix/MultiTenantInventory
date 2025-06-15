using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Domain.Entities;
using MultiTenantInventory.Infrastructure.Extensions;
using MultiTenantInventory.Infrastructure.Persistence;

namespace MultiTenantInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController: ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ITenantProvider _tenantProvider;
        public ProductsController(AppDbContext dbContext, ITenantProvider tenant)
        {
            _dbContext = dbContext;
            _tenantProvider = tenant;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //var tenant = _tenantProvider.GetTenant();
            var tenantId = User.GetTenantId();
            if (tenantId == null) return Unauthorized(new ApiResponse<string>("Tenant could not be resolved."));
            var products = await _dbContext.Products
                .Where(p => p.TenantId == tenantId)
                .ToListAsync();
            return Ok(new ApiResponse<List<Product>>(products, "All Products Fetched Successfully"));
        }
    }
}
