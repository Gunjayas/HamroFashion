namespace HamroFashion.Api.V1.Commands
{
    public record ChangePassword
    {
        public string Password { get; set; }

        public Guid UserId { get; set; }
    }
}
