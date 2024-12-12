using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record CartModel : BaseModel
    {
        public UserModel User { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<CartItemModel> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
