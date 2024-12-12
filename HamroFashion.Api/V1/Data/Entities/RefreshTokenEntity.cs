namespace HamroFashion.Api.V1.Data.Entities
{
    /// <summary>
    /// Represents a refresh token, a code that can be used with an expired bearer
    /// to refresh access code
    /// </summary>
    public record RefreshTokenEntity : BaseEntity
    {
        /// <summary>
        /// UTC Date and time when this refresh token expires (should sign back in)
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// A longer lived token that can be used to obtain a new bearer token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// The UserIdentity that this refresh token authenticates
        /// </summary>
        public UserEntity? User { get; set; }

        /// <summary>
        /// Unique id of the UserIdentity that this refresh token is for
        /// </summary>
        public Guid UserId { get; set; }
    }
}
