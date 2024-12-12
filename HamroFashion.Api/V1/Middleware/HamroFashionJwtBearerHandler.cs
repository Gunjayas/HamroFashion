using HamroFashion.Api.V1.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using static HamroFashion.Api.V1.Data.HamroFashionContext;

namespace HamroFashion.Api.V1.Middleware
{
    /// <summary>
    /// We override Jwt bearer token handling in the ASP pipeline
    /// </summary>
    public class HamroFashionJwtBearerHandler : JwtBearerHandler
    {
        /// <summary>
        /// Internally used HamroFashion database context (a Transient one so we know it's clean and we don't interfere 
        /// with the normal minimal api pipeline)
        /// </summary>
        public readonly TransientHamroFashionContext DB;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="db">Needed so we can pull token configurations</param>
        /// <param name="options">Pass through for normal auth functionality</param>
        /// <param name="logger">Pass through for normal auth functionality</param>
        /// <param name="encoder">Pass through for normal auth functionality</param>
        public HamroFashionJwtBearerHandler(TransientHamroFashionContext db, IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
            DB = db;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), JwtBearerDefaults.AuthenticationScheme));

            // Get the token from the Authorization header
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                return AuthenticateResult.Fail("Authorization header not found");
            }

            var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer"))
                return AuthenticateResult.Fail("Bearer token not found in Authorization header");

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var principal = GetClaims(token);

            if (principal == null)
                return AuthenticateResult.Fail("Invalid bearer token, no claims principal found");

            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type.Equals("userId", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(userIdClaim?.Value) || !Guid.TryParse(userIdClaim.Value, out var userId))
                return AuthenticateResult.Fail("Bearer token does not contain a valid user id");

            var user = await DB.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return AuthenticateResult.Fail("Bearer token does not point to a valid user");

            if (!ValidateToken(token, user))
                return AuthenticateResult.Fail("Invalid token");

            Context.User = principal;
            return AuthenticateResult.Success(new AuthenticationTicket(principal, JwtBearerDefaults.AuthenticationScheme));

        }

        /// <summary>
        /// Internal method that we use to perform the token validation
        /// </summary>
        /// <param name="token">The bearer token sent by user</param>
        /// <param name="options">The TokenOptions we should use to validate with</param>
        /// <returns>bool</returns>
        private bool ValidateToken(string token, UserEntity user)
        {
            string secretKey = "abcdefghijklmnopqrstuvwxyz123456";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var validator = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = "hamrofashion.com",
                ValidIssuer = "hamrofashion.com"
            };

            try
            {
                validator.ValidateToken(token, validationParams, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Internal method that we use to convert the token string to a claims principal object
        /// </summary>
        /// <param name="authToken">The bearer token provided by the </param>
        /// <returns>ClaimsPrincipal loaded from auth token</returns>
        private ClaimsPrincipal? GetClaims(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(authToken) as JwtSecurityToken;

            if (token == null)
                return null;

            var claimsIdentity = new ClaimsIdentity(token.Claims, "Token");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}
