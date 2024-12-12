using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record CartItemModel : BaseModel
    {
        public CartModel Cart { get; set; }
        public Guid CartId { get; set; }
        public ProductModel Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        // Price at the time of adding to cart, to notify user if the price changes when moving cartitems to order
        public decimal Price { get; set; }
    }
}
