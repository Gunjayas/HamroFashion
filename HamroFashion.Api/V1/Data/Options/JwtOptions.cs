namespace HamroFashion.Api.V1.Data.Options
{
    /// <summary>
    /// JWT options we load from config
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// The JWT aud
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The minutes before a token expires (from the time it's created)
        /// </summary>
        public int BearerExpiryMinutes { get; set; }

        /// <summary>
        /// The JWT iss
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Base64 encoded JWT signing key
        /// </summary>
        public string SigningKey { get; set; }
    }
}
