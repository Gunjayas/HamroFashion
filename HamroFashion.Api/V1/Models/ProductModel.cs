using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record ProductModel : BaseModel
    {
        public UserEntity CreatedBy { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        public string[] ImageUrls { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; } = true;
        public TagModel? Category { get; set; }
        public IEnumerable<TagModel>? Collection { get; set; }
        public IEnumerable<TagModel>? Label { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductModel() : base() { }
    }
}
