namespace HamroFashion.Api.V1.Data.Entities
{
    public record Role : BaseEntity
    {
        /// <summary>
        /// Human friendly description of this role, what permissions it grants
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The per scope unique name of this role
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Navigation property to the permissions granted by this role
        /// </summary>
        public IEnumerable<RolePermission>? Permissions { get; set; }

        /// <summary>
        /// Navigation property to the user identities that are assigned this role
        /// </summary>
        public IEnumerable<UserRole>? Users { get; set; }
    }
}
