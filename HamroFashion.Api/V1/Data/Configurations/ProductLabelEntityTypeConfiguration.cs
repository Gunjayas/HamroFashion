using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class ProductLabelEntityTypeConfiguration : IEntityTypeConfiguration<ProductLabel>
    {
        public void Configure(EntityTypeBuilder<ProductLabel> builder)
        {
            builder.ConfigureBaseModel();

            builder.HasOne(x => x.Tag);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductLabel);
        }
    }
}
