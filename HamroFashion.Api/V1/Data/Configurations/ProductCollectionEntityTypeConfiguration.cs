using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class ProductCollectionEntityTypeConfiguration : IEntityTypeConfiguration<ProductCollection>
    {
        public void Configure(EntityTypeBuilder<ProductCollection> builder)
        {
            builder.ConfigureBaseModel();

            builder.HasOne(x => x.Tag);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductCollection);
        }
    }
}
