using MultiTenantInventory.Domain.Entities;

namespace MultiTenantInventory.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByTenantIdAsync(Guid tenantId);
    }
}
