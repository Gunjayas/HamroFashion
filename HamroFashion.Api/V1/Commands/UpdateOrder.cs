namespace HamroFashion.Api.V1.Commands
{
    public record UpdateOrder : PlaceOrder
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
