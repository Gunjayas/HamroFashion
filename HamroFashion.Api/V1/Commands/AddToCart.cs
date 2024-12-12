namespace HamroFashion.Api.V1.Commands
{
    public record AddToCart
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
