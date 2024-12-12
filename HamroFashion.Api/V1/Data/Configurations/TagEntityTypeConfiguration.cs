using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HamroFashion.Api.V1.Data.Configurations
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ConfigureBaseModel();

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);

            builder.Property(x => x.Name)
                .HasMaxLength(40)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(x => x.TagType)
                .HasMaxLength(40)
                .IsRequired()
                .IsUnicode(false);

            builder.HasIndex(x => new { x.TagType, x.Name })
                .IsUnique();

            builder.HasData(new Tag[]
            {
                new Tag
                {
                    Description = "Adventure games are more interactive",
                    Name = "Lehenga",
                    TagType = TagType.Category,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Fantasy games are mythical",
                    Name = "Kurtha Surwal",
                    TagType = TagType.Category,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Games that take place in medieval times / worlds",
                    Name = "Leggings",
                    TagType = TagType.Category,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Forums that are Educational, mostly learning resources",
                    Name = "Festive",
                    TagType = TagType.Label,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Forums that are posted to seek answer or advice",
                    Name = "Ethnic",
                    TagType = TagType.Label,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Forum Posts that are related to technological advances and news",
                    Name = "Festive Collection",
                    TagType = TagType.Collection,
                    Id = Guid.NewGuid()
                },

                new Tag
                {
                    Description = "Forum Posts that are related to technological advances and news",
                    Name = "Winter Wears",
                    TagType = TagType.Collection,
                    Id = Guid.NewGuid()
                }
            });
        }
    }
}
