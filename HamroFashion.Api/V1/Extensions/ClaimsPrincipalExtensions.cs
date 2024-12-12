using Newtonsoft.Json;
using System.Security.Claims;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// Some helper extensions for getting Mudmunity claims from a ClaimsPrincipal
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Helper to get the userId claim value, as a guid.
        /// </summary>
        /// <param name="principal">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <returns>The user id as a guid, or throws exception</returns>
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdString = principal.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var userId = Guid.Parse(userIdString);
            return userId;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            var userNameString = principal.Claims.FirstOrDefault(x => x.Type == "userName")?.Value;
            return userNameString;
        }

        /// <summary>
        /// Check to see if the provided claims principal has the requested role
        /// </summary>
        /// <param name="claims">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <param name="role">The user role</param>
        /// <returns>bool</returns>
        public static bool HasRole(this ClaimsPrincipal principal)
        {
            var siteRoles = principal.Claims.FirstOrDefault(x => x.Type == "userRoles")?.Value;

            if (siteRoles == null)
                return false;

            return true;
        }

        /// <summary>
        /// Helper to get the userRole claim value, as a guid.
        /// </summary>
        /// <param name="principal">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <returns>The user role as a string, or throws exception</returns>
        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            var userRoles = principal.Claims.FirstOrDefault(x => x.Type == "userRoles")?.Value;

            return userRoles;
        }

        /// <summary>
        /// Check to see if the provided claims principal has the requested id
        /// </summary>
        /// <param name="claims">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <param name="id">The unique id of the permission</param>
        /// <returns>bool</returns>
        public static bool HasPermission(this ClaimsPrincipal claims, Guid id)
        {
            var permissions = GetPermissionDictionary(claims);
            if (permissions == null)
                return false;

            return permissions?.Any(x => x.Key.Equals(id.ToString(), StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        /// <summary>
        /// Check to see if the provided claims principal has the requested title
        /// </summary>
        /// <param name="claims">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <param name="title">The title (ScopeType:Scope Name:Permission Name) of the permission</param>
        /// <returns>bool</returns>
        public static bool HasPermission(this ClaimsPrincipal claims, string title)
        {
            var permissions = GetPermissionDictionary(claims);
            if (permissions == null)
                return false;

            return permissions.Any(x =>
                x.Value.Equals(title, StringComparison.OrdinalIgnoreCase)
            );
        }

        /// <summary>
        /// Check to see if the provided claims principal is authenticated
        /// </summary>
        /// <param name="principal">The HttpContext.User ClaimsPrincipal for the incoming request</param>
        /// <returns>bool</returns>
        public static bool IsSignedIn(this ClaimsPrincipal principal)
        {
            return principal?.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// Helper method used by above extensions, this will get and parse the permissions dictionary
        /// from the provided claims principal
        /// </summary>
        /// <param name="claims">The ClaimsPrincipal to get permissions dictionary from</param>
        /// <returns>Dictionary(string, string) of { Permission.Id, Title }</returns>
        private static Dictionary<string, string>? GetPermissionDictionary(ClaimsPrincipal claims)
        {
            var permissionJson = claims.FindFirst(x => x.Type.Equals("permissions", StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrWhiteSpace(permissionJson))
                return null;

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(permissionJson);
        }

    }
}
