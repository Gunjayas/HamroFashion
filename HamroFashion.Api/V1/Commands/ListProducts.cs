namespace HamroFashion.Api.V1.Commands
{
    /// <summary>
    /// Command of optional sorting and filtering of products
    /// </summary>
    public record ListProducts
    {
        public string? Category { get; set; }
        public string? Label { get; set; }
        public string? Color { get; set; }
        public string? Collection { get; set; }
        public string? Search {  get; set; }
        public Guid? UserId { get; set; }
    }
}
