namespace HamroFashion.Api.V1.Data.Entities
{
    public record Permission : BaseEntity
    {
        /// <summary>
        /// Human friendly description of what this permission allows the user to do
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Unique per scope name of this permission
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Navigation property to the user permissions that grant this permission to users
        /// </summary>
        public IEnumerable<UserPermission> Users { get; set; }

        /// <summary>
        /// Navigation property to the roles that grant this permission
        /// </summary>
        public IEnumerable<RolePermission> Roles { get; set; }


    }
}
