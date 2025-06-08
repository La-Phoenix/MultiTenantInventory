using AutoMapper;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Application.DTOs;

namespace MultiTenantInventory.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ITenantProvider tenantProvider, IMapper mapper)
        {
            _productRepository = productRepository;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetProductForTenantsAsync()
        {
            var tenant = _tenantProvider.GetTenant() ?? throw new InvalidOperationException("Tenant is not resolved.");
            
            var products = await _productRepository.GetProductsByTenantIdAsync(tenant.Id);

            // Map the products to a DTO or return them directly
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
