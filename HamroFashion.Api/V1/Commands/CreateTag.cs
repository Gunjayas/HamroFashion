namespace HamroFashion.Api.V1.Commands
{
    public record CreateTag
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TagType { get; set; }
    }
}
