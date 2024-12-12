using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record ProductEntity : BaseEntity
    {
        public UserEntity CreatedBy { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        public string[] ImageUrls { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; } = true;
        public decimal Price { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public IEnumerable<ProductCollection>? ProductCollection { get; set; }
        public IEnumerable<ProductLabel>? ProductLabel { get; set; }

    }

    public static class ProductEntityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static ProductModel ToModel(this ProductEntity entity)
        {
            var model = new ProductModel
            {
                Name = entity.Name,
                Description = entity.Description,
                Color = entity.Color,
                ImageUrls = entity.ImageUrls,
                Size = entity.Size,
                Quantity = entity.Quantity,
                Availability = entity.Availability
            };

            if (entity.ProductCategory != null)
            {
                model.Category = entity.ProductCategory.Tag != null
                    ? new TagModel
                    {
                        Name = entity.ProductCategory.Tag.Name,
                        Description = entity.ProductCategory.Tag.Description,
                        TagType = entity.ProductCategory.Tag.TagType,
                    }
                    : null;
            }

            if (entity.ProductLabel != null)
            {
                model.Label = entity.ProductLabel.Any(gg => gg.Tag != null)
                    ? entity.ProductLabel.Select(x => new TagModel
                    {
                        Name = x.Tag.Name,
                        Description = x.Tag.Description,
                        TagType = x.Tag.TagType
                    }).ToList() 
                    : null;
            }

            if (entity.ProductCollection != null)
            {
                model.Collection = entity.ProductCollection.Any(gg => gg.Tag != null)
                    ? entity.ProductCollection.Select(x => new TagModel
                    {
                        Name = x.Tag.Name,
                        Description = x.Tag.Description,
                        TagType = x.Tag.TagType
                    }).ToList()
                    : null;
            }

            return model;
        }

        /// <summary>
        /// Converts the entire collection of entities to DTO models
        /// </summary>
        /// <param name="entities">The collection of entities to convert</param>
        /// <returns>Collection of DTO models representing these entities</returns>
        public static IEnumerable<ProductModel> ToModel(this IEnumerable<ProductEntity> entities)
            => entities.Select(x => x.ToModel());
    }
}
