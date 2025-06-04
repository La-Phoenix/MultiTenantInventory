using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Infrastructure.Persistence.EntityConfigurations
{
    /// <summary>
    /// Configures the Product entity for EF Core using Fluent API.
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Set the table name for the Product entity
            builder.ToTable("products");

            // Primary key configuration
            builder.HasKey(p => p.Id);

            // Set default value for Id using PostgreSQL's gen_random_uuid() function
            builder.Property(p => p.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            // Product Name: Required with a max length of 200 characters
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Product Description: Optional with a max length of 1000 characters
            builder.Property(p => p.SKU)
                   .IsRequired()
                   .HasMaxLength(100);

            // Product SKU: Required with a max length of 100 characters
            builder.HasIndex(p => p.SKU)
                   .IsUnique();

            // Product Description: Optional with a max length of 1000 characters
            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Product Description: Optional with a max length of 1000 characters
            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            // Product Quantity: Required
            builder.Property(p => p.Quantity)
                   .IsRequired();

            // Product TenantId: Required foreign key to the Tenant entity
            builder.Property(p => p.TenantId)
                   .IsRequired();
        }
    }
}
