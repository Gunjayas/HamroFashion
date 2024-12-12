using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record OrderItem : BaseEntity
    {
        public OrderEnity Order { get; set; }
        public Guid OrderId { get; set; }
        public ProductEntity Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        // Price at the time of adding to cart, to notify user if the price changes when moving cartitems to order
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class OrderItemExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static OrderItemModel ToModel(this OrderItem entity)
            => new OrderItemModel
            {
                Order = entity.Order?.ToModel(),
                ProductId = entity.ProductId,
                OrderId = entity.OrderId,
                Product = entity.Product?.ToModel()
            };

        public static IEnumerable<OrderItemModel> ToModel(this IEnumerable<OrderItem> entities)
            => entities.Select(x => x.ToModel());
    }
}
