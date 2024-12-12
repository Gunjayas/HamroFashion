namespace HamroFashion.Api.V1.Commands
{
    public class UpdateCartItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
