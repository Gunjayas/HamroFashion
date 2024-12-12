namespace HamroFashion.Api.V1.Models
{
    /// <summary>
    /// Represents a successful sign in response
    /// </summary>
    public class AuthenticationSuccessful
    {
        /// <summary>
        /// The bearer token the caller can use in Auth header of rest calls
        /// </summary>
        public string? BearerToken { get; set; }

        /// <summary>
        /// UTC Date and time when this bearer token expires (should use refresh
        /// token to get a new bearer token)
        /// </summary>
        public DateTime BearerTokenExpires { get; set; }

        /// <summary>
        /// A longer lived token that can be used to obtain a new bearer token
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// UTC Date and time when this refresh token expires (should sign back in)
        /// </summary>
        public DateTime RefreshTokenExpires { get; set; }

        /// <summary>
        /// The UserIdentity that this sign in response authenticates
        /// </summary>
        public UserModel User { get; set; }
    }
}
