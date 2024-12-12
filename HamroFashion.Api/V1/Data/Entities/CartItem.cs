using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record CartItem : BaseEntity
    {
        public CartEntity Cart { get; set; }
        public Guid CartId { get; set; }
        public ProductEntity Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        // Price at the time of adding to cart, to notify user if the price changes when moving cartitems to order
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class CartItemExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static CartItemModel ToModel(this CartItem entity)
            => new CartItemModel
            {
                Cart = entity.Cart?.ToModel(),
                ProductId = entity.ProductId,
                CartId = entity.CartId,
                Product = entity.Product?.ToModel()
            };

        public static IEnumerable<CartItemModel> ToModel(this IEnumerable<CartItem> entities)
            => entities.Select(x => x.ToModel());
    }
}
