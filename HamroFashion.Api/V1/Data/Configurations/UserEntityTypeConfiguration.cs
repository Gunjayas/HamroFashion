using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    /// <summary>
    /// Configures our UserIdentity model for EF
    /// </summary>
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            //builder.ToTable("UserIdentities", "Auth");

            builder.ConfigureBaseModel();

            builder.Property(x => x.UserName)
                .HasMaxLength(128)
                .IsUnicode(false);

            builder.Property(x => x.EmailAddress)
                .HasMaxLength(128)
                .IsUnicode(false);

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(256);

            builder.HasMany(x => x.UserPermissions)
                .WithOne(x => x.User);

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User);

            builder.HasMany(x => x.WishListItems)
                .WithOne(x => x.User);
        }
    }
}
