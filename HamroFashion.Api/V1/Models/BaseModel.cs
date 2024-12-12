namespace HamroFashion.Api.V1.Models
{
    /// <summary>
    /// BaseModel represents the common fields of most or all models in the HamroFashion service
    /// </summary>
    public record BaseModel
    {
        /// <summary>
        /// Unique id of the user account that first saved this model to the database
        /// </summary>
        public Guid? CreatedById { get; set; } = null;

        /// <summary>
        /// Date and time (UTC) when this model was first saved to the database
        /// </summary>
        public DateTime? CreatedOn { get; set; } = null;

        /// <summary>
        /// Unique id of this model
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Unique id of the user account that last updated this model in the database
        /// </summary>
        public Guid? ModifiedBy { get; set; } = null;

        /// <summary>
        /// Date and time (UTC) when this model was last updated in the database
        /// </summary>
        public DateTime? ModifiedOn { get; set; } = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseModel() { }
    }
}
