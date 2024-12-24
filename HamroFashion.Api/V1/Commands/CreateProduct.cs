namespace HamroFashion.Api.V1.Commands
{
    /// <summary>
    /// CQRS Command representing a request to create a new Product
    /// </summary>
    public record CreateProduct
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        //temporary optional
        public string? ImageUrl { get; set; }
        public string? Size { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; } = true;

        /// <summary>
        /// Unique id's of the tags to apply to this product
        /// </summary>
        public ICollection<Guid>? ProductCollection { get; set; }
        public ICollection<Guid>? ProductLabel { get; set; }
        public string? ProductCategory { get; set; }
    }
}
