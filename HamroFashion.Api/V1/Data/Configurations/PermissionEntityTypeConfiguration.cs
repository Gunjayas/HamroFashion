using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    /// <summary>
    /// Configures EF for our Permission 
    /// </summary>
    public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
    {
        /// <summary>
        /// Configures EF for our Permission model
        /// </summary>
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ConfigureBaseModel();

            builder.Property(x => x.Description)
                .HasMaxLength(5000)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true)
                .IsUnicode(false);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Permission);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Permission);

            builder.HasData([
                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "View all",
                    Id = SeedData.ViewAllPermissionId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "ViewAll",
                },
                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Can Post",
                    Id = SeedData.CanPost,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "CanPost",
                },

                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Can delete any entity",
                    Id = SeedData.DeletePermissionId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "Delete"
                },

                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Can Order",
                    Id = SeedData.CanOrderPermissionId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "CanOrder"
                },

                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Can Return",
                    Id = SeedData.CanReturnPermissionId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "CanReturn"
                },

                new Permission
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Can Manage Orders",
                    Id = SeedData.CanManageOrderPermissionId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "CanManageOrders"
                }
            ]);
        }
    }
}
