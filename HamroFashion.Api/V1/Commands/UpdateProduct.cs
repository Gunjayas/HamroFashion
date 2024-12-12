namespace HamroFashion.Api.V1.Commands
{
    public record UpdateProduct : CreateProduct
    {
        public Guid Id { get; set; }
    }
}
