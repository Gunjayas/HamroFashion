using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record OrderModel : BaseModel
    {
        public UserModel User { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

}
