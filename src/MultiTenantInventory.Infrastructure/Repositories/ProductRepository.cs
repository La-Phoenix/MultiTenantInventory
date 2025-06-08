
using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Domain.Entities;
using MultiTenantInventory.Infrastructure.Persistence;

namespace MultiTenantInventory.Infrastructure.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetProductsByTenantIdAsync(Guid tenantId)
        {
            // Example implementation
            return await _dbContext.Products
                .Where(p => p.TenantId == tenantId)
                .ToListAsync();
        }
    }
}
