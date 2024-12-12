namespace HamroFashion.Api.V1.Data.Entities
{
    /// <summary>
    /// BaseEntity represents the common fields of most or all entity in the HamroFashion
    /// </summary>
    public record BaseEntity
    {
        /// <summary>
        /// Unique id of the user account that first saved this entity to the database
        /// </summary>
        public Guid? CreatedById { get; set; } = null;

        /// <summary>
        /// Date and time (UTC) when this entity was first saved to the database
        /// </summary>
        public DateTime? CreatedOn { get; set; } = null;

        /// <summary>
        /// Unique id of this entity
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and time (UTC) when this entity was last updated in the database
        /// </summary>
        public Guid? ModifiedBy { get; set; } = null;

        /// <summary>
        /// Unique id of the user account that last updated this entity in the database
        /// </summary>
        public DateTime? ModifiedOn { get; set; } = null;
    }
}
