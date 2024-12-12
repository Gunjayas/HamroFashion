namespace HamroFashion.Api.V1.Commands
{
    public record UpdateUser : CreateUser
    {
        public Guid Id { get; set; }
    }
}
