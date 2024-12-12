using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record WishListItem : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class WishListItemExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static WishListItemModel ToModel(this WishListItem entity)
            => new WishListItemModel
            {
                User = entity.User?.ToModel(),
                Product = entity.Product?.ToModel(),
                ProductId = entity.ProductId,
                UserId = entity.UserId
            };

        public static IEnumerable<WishListItemModel> ToModel(this IEnumerable<WishListItem> entities)
            => entities.Select(x => x.ToModel());
    }
}
