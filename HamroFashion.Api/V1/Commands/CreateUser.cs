namespace HamroFashion.Api.V1.Commands
{
    public record CreateUser
    {
        public string? ProfilePicture { get; set; }
        
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
