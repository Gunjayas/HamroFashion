using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record CartEntity : BaseEntity
    {
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class CartEntityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static CartModel ToModel(this CartEntity entity)
            => new CartModel
            {
                User = entity.User?.ToModel(),
                CartItems = entity.CartItems?.ToModel(),
                UserId = entity.UserId,
                TotalPrice = entity.TotalPrice
            };

        public static IEnumerable<CartModel> ToModel(this IEnumerable<CartEntity> entities)
            => entities.Select(x => x.ToModel());
    }
}
