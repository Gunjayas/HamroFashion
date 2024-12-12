namespace HamroFashion.Api.V1.Commands
{
    public record GetUser
    {
        public string? EmailAddress { get; set; }

        public string? Password { get; set; }
    }
}
