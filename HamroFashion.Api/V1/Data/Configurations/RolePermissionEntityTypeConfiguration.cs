using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class RolePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        /// <summary>
        /// Configures EF for our RolePermission model
        /// </summary>
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ConfigureBaseModel();

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.Roles);
            //.OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Permissions);
            //.OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
            new RolePermission
            {
                Id = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow,
                RoleId = SeedData.UserRoleId, // ID of User role
                PermissionId = SeedData.ViewAllPermissionId // ID of ViewAll permission
            },
            new RolePermission
            {
                Id = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow,
                RoleId = SeedData.ModRoleId, // ID of User role
                PermissionId = SeedData.CanPost // ID of ViewAll permission
            },
            new RolePermission
            {
                Id = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow,
                RoleId = SeedData.ModRoleId, // ID of Moderator role
                PermissionId = SeedData.DeletePermissionId // ID of Delete permission
            },
            new RolePermission
            {
                Id = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = Guid.NewGuid(),
                ModifiedOn = DateTime.UtcNow,
                RoleId = SeedData.UserRoleId,
                PermissionId = SeedData.CanOrderPermissionId
            }
        );
        }
    }
}
