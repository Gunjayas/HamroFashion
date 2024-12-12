using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Configures EF for our role model
        /// </summary>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ConfigureBaseModel();

            builder.Property(x => x.Description)
                .HasMaxLength(5000)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true)
                .IsUnicode(true);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Role);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Role);

            builder.HasData([
                new Role
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "User",
                    Id = SeedData.UserRoleId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "User"
                },
                new Role
                {
                    CreatedById = new Guid(),
                    CreatedOn = DateTime.UtcNow,
                    Description = "Moderator",
                    Id = SeedData.ModRoleId,
                    ModifiedBy = new Guid(),
                    ModifiedOn = DateTime.UtcNow,
                    Name = "Mod"
                }
            ]);
        }
    }
}
