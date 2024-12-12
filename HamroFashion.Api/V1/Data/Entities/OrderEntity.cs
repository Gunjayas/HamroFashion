using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public record OrderEnity : BaseEntity
    {
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class OrderEnityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static OrderModel ToModel(this OrderEnity entity)
            => new OrderModel
            {
                User = entity.User?.ToModel(),
                OrderItems = entity.OrderItems?.ToModel(),
                UserId = entity.UserId
            };

        public static IEnumerable<OrderModel> ToModel(this IEnumerable<OrderEnity> entities)
            => entities.Select(x => x.ToModel());
    }
}
