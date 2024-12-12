using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    /// <summary>
    /// Configures EF for our product model
    /// </summary>
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        /// <summary>
        /// Configures EF for our product model
        /// </summary>
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ConfigureBaseModel();

            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsUnicode(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true)
                .IsUnicode(false);

            builder.HasMany(x => x.ProductLabel)
                .WithOne(x => x.Product);

            builder.HasMany(x => x.ProductCollection)
                .WithOne(x => x.Product);
        }
    }
}
