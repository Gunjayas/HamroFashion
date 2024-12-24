using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    /// <summary>
    /// Basic site roles
    /// </summary>
    public static class SiteRoles
    {
        /// <summary>
        /// Admins can do whateva they want
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Moderators can help moderate sales, quantity etc
        /// </summary>
        public const string Moderator = "Moderator";

        /// <summary>
        /// Users can post content, Games will require Moderator approval
        /// </summary>
        public const string User = "User";
    }

    /// <summary>
    /// Represents a UserIdentity or user account in our platform
    /// </summary>
    public record UserEntity : BaseEntity
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

        public IEnumerable<UserPermission>? UserPermissions { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }
        public IEnumerable<WishListItem>? WishListItems { get; set; }
        public CartEntity Cart { get; set; } = new CartEntity();

    }

    public static class UserEntityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static UserModel ToModel(this UserEntity entity)
        {
            var model = new UserModel
            {
                Id = entity.Id,
                PasswordHash = entity.PasswordHash,
                EmailAddress = entity.EmailAddress,
                UserName = entity.UserName,
                ProfilePicUrl = entity.ProfilePicUrl
            };

            return model;
        }

        /// <summary>
        /// Converts the entire collection of entities to DTO models
        /// </summary>
        /// <param name="entities">The collection of entities to convert</param>
        /// <returns>Collection of DTO models representing these entities</returns>
        public static IEnumerable<UserModel> ToModel(this IEnumerable<UserEntity> entities) 
            => entities.Select(x => x.ToModel());
    }
}
