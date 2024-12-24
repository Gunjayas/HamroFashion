namespace HamroFashion.Api.V1.Commands
{
    public class PaymentRequest
    {
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
