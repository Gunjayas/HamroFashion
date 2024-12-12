using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// Configures EF for our UserRole model
        /// </summary>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ConfigureBaseModel();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoles);
        }
    }
}
