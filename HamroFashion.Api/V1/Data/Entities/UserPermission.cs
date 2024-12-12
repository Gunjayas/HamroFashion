namespace HamroFashion.Api.V1.Data.Entities
{
    public record UserPermission : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
