namespace HamroFashion.Api.V1.Models
{
    public record TagModel : BaseModel
    {
        /// <summary>
        /// Human friendly description of this tag
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Unique name of this tag
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The type of tag this is
        /// </summary>
        public required string TagType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TagModel() : base() { }

    }
}
