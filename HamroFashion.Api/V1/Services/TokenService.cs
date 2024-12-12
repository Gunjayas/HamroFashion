using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using HamroFashion.Api.V1.Data.Entities;
using HamroFashion.Api.V1.Exceptions;
using HamroFashion.Api.V1.Extensions;
using HamroFashion.Api.V1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace HamroFashion.Api.V1.Services
{
    /// <summary>
    /// Our token issuer service. Use this to authenticate, generate and manage auth tokens
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// The HamroFashionContext we persist to
        /// </summary>
        public readonly HamroFashionContext dbContext;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="dbContext">The MudmunityContext to use</param>
        public TokenService(HamroFashionContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AuthenticationSuccessful> FromCredentials(GetUser command, CancellationToken cancellationToken = default)
        {
            var user = await dbContext.Users
                .Where(x => x.EmailAddress == command.EmailAddress)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                if (user.PasswordHash !=null && !string.IsNullOrWhiteSpace(command.Password) && command.Password.EqualsPasswordHash(user.PasswordHash))
                {
                    var refreshToken = new RefreshTokenEntity
                    {
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        UserId = user.Id
                    };
                    
                    dbContext.RefreshTokens.Add(refreshToken);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    return new AuthenticationSuccessful
                    {
                        BearerToken = await GenerateJwtBearerTokenAsync(user),
                        BearerTokenExpires = DateTime.UtcNow.AddMinutes(60),
                        RefreshToken = refreshToken.Token,
                        RefreshTokenExpires = DateTime.UtcNow.AddMinutes(60),
                        User = user.ToModel()
                    };
                }
            }
            throw new ApiException("Failed to sign in", new Dictionary<string, string[]>
            {
                { "emailAddress", [ "Invalid email address or password" ] },
                { "password", [ "Invalid email address or password" ] }
            });
        }

        /// <summary>
        /// Helper method that generates the actual JWT bearer token
        /// </summary>
        /// <param name="user">The user account to generate token for</param>
        /// <param name="tenant">The tenant token options to use for signing the token</param>
        /// <returns></returns>
        protected async Task<string> GenerateJwtBearerTokenAsync(UserEntity user)
        {
            string secretKey = "abcdefghijklmnopqrstuvwxyz123456";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var handler = new JwtSecurityTokenHandler();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var permissions = new Dictionary<string, string>();

            var perm1 = await dbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .SelectMany(ur => ur.Role.Permissions)
                .Select(rp => new {rp.PermissionId, rp.Permission.Name})
                .ToListAsync();

            foreach (var userPermission in perm1)
            {
                permissions.Add(userPermission.PermissionId.ToString(), userPermission.Name);
            }

            if(user.UserPermissions != null && user.UserPermissions.Count() > 0)
            {
                var perm2 = user.UserPermissions.Select(rp => new { rp.PermissionId, rp.Permission.Name}).ToList();

                foreach (var userPermission in perm2)
                {
                    permissions.Add(userPermission.PermissionId.ToString(), userPermission.Name);
                }
            }

            var claims = new Dictionary<string, object>
            {
                { "userId", user.Id.ToString() },
                { "permissions", permissions }
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Audience = "hamrofashion.com",
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(15),
                IssuedAt = DateTime.UtcNow,
                Issuer = "hamrofashion.com",
                SigningCredentials = creds
            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
