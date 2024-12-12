using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record ProductLabel : BaseEntity
    {
        public virtual ProductEntity Product { get; set; }

        public Guid ProductId { get; set; }

        public virtual Tag Tag { get; set; }

        public Guid TagId { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class ProductLabelEntityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static ProductLabelModel ToModel(this ProductLabel entity)
            => new ProductLabelModel
            {
                Tag = entity.Tag,
                Product = entity.Product,
                ProductId = entity.ProductId,
                TagId = entity.TagId
            };

        public static IEnumerable<ProductLabelModel> ToModel(this IEnumerable<ProductLabel> entities)
            => entities.Select(x => x.ToModel());
    }
}
