using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record WishListItemModel : BaseModel
    {
        public Guid UserId { get; set; }
        public UserModel User { get; set; }
        public Guid ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
