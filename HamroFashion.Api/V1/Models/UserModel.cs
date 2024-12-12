using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{

    /// <summary>
    /// Represents a UserIdentity or user account in our platform
    /// </summary>
    public record UserModel : BaseModel
    {
        public string? ProfilePicUrl { get; set; }
        /// <summary>
        /// User chosen, unique name
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Unique email address for this user identity
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Hashed password value for this user identity
        /// </summary>
        public byte[]? PasswordHash { get; set; }

        /*public IEnumerable<UserPermission>? UserPermissions { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }*/

    }

}
