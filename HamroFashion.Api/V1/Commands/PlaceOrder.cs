namespace HamroFashion.Api.V1.Commands
{
    public record PlaceOrder
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string PaymentMethod { get; set; }
        public Guid CartId { get; set; }
    }
}
