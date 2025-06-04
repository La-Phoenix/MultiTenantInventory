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
    /// Configures the User entity for EF Core using Fluent API.
    /// </summary>
    public class UserConfiguration: IEntityTypeConfiguration<Domain.Entities.User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Set the table name for the User entity
            builder.ToTable("users");

            // Primary key configuration
            builder.HasKey(u => u.Id);

            // Set default value for Id using PostgreSQL's gen_random_uuid() function
            builder.Property(u => u.Id)
                   .HasDefaultValueSql("gen_random_uuid()");

            // User Email: Required with a max length of 256 characters
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            // User Email: Unique index to ensure no two users can have the same email
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            // User FullName: Required with a max length of 200 characters
            builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            // User PasswordHash: Required with a max length of 512 characters
            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            // User TenantId: Required foreign key to the Tenant entity
            builder.Property(u => u.TenantId)
                   .IsRequired();
        }
    }
}
