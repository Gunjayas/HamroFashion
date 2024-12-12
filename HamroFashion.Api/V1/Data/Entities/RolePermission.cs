namespace HamroFashion.Api.V1.Data.Entities
{
    public record RolePermission : BaseEntity
    {
        /// <summary>
        /// Navigation property to the permission granted by this role
        /// </summary>
        public Permission? Permission { get; set; }

        /// <summary>
        /// Unique id of the permission granted by this role
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Navigation property to the role that grants this permission
        /// </summary>
        public Role? Role { get; set; }

        /// <summary>
        /// Unique id of the role that grants this permission
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
