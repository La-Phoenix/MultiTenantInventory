using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Persistence.EntityConfigurations
{
    /// <summary>
    /// Configures the Tenant entity for EF Core using Fluent API.
    /// </summary>
    public class TenantConfiguration: IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // Set the table name for the Tenant entity
            builder.ToTable("Tenants");


            // Primary key configuration
            builder.HasKey(t => t.Id);
            // Set default value for Id using PostgreSQL's gen_random_uuid() function
            builder.Property(t => t.Id)
                .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL UUID gen

            // Tenant Name: Required with a max length of 200 characters
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Tenant Subdomain: Required with a max length of 100 characters
            builder.Property(t => t.Subdomain)
                .IsRequired()
                .HasMaxLength(100);

            // Unique index on Subdomain to ensure no two tenants can have the same subdomain
            builder.HasIndex(t => t.Subdomain)
                .IsUnique();

            // Define one-to-many relationships with Users
            builder.HasMany(t => t.Users)
                .WithOne(u => u.Tenant!)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationships with Products
            builder.HasMany(t => t.Products)
                .WithOne(p => p.Tenant!)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
