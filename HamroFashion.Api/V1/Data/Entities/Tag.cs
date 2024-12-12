using HamroFashion.Api.V1.Models;

namespace HamroFashion.Api.V1.Data.Entities
{
    public static class TagType
    {
        public static string Collection = "Collection";

        public static string Category = "Category";

        public static string Label = "Label";
    }
    public record Tag : BaseEntity
    {
        /// <summary>
        /// Human friendly description of this tag
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique name of this tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of tag this is
        /// </summary>
        public string TagType { get; set; }

    }

    /// <summary>
    /// Entity extensions
    /// </summary>
    public static class TagEntityExtensions
    {
        /// <summary>
        /// Convert to DTO Model
        /// </summary>
        /// <param name="entity">The entity to convert</param>
        /// <returns>The DTO model representation of this entity</returns>
        public static TagModel ToModel(this Tag entity)
            => new TagModel
            {
                Description = entity.Description,
                Name = entity.Name,
                TagType = entity.TagType
            };

        public static IEnumerable<TagModel> ToModel(this IEnumerable<Tag> entities)
            => entities.Select(x => x.ToModel());
    }
}
