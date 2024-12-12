namespace HamroFashion.Api.V1.Commands
{
    public record PlaceOrder
    {
        public Guid CartId { get; set; }
        public string Address { get; set; }
        public string Cuppon { get; set; }
        public string PaymentMethod { get; set; }
    }
}
