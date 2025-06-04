using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Domain.Entities;
using MultiTenantInventory.Infrastructure.Persistence.EntityConfigurations;
using MultiTenantInventory.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Persistence
{
    public class AppDbContext: DbContext
    {

        // Exported DbSet for each aggregate root
        public DbSet<Tenant> Tenants { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;

        
        private readonly ITenantProvider _tenantProvider;
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all entity configurations
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            // Example: Global Query Filter for multi‐tenancy on User & Product
            // (TenantId property is on User & Product entities)
            //
            // Note: _tenantProvider.GetTenantId()will return the current tenant's GUID.
            //
            // modelBuilder.Entity<User>()
            //     .HasQueryFilter(u => u.TenantId == _tenantProvider.GetTenantId());
            //
            // modelBuilder.Entity<Product>()
            //     .HasQueryFilter(p => p.TenantId == _tenantProvider.GetTenantId());

            base.OnModelCreating(modelBuilder);
        }
    }
}
