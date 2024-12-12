namespace HamroFashion.Api.V1.Data.Entities
{
    public record UserRole : BaseEntity
    {
        /// <summary>
        /// Navigation property to the role assigned to this user
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Unique id of the role assigned to this user
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Navigation property to the user that this role is assigned to
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// Unique id of the user that this role is assigned to
        /// </summary>
        public Guid UserId { get; set; }
    }
}
