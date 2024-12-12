using HamroFashion.Api.V1.Data.Entities;

namespace HamroFashion.Api.V1.Models
{
    public record ProductLabelModel : BaseModel
    {
        public virtual ProductEntity Product { get; set; }

        public Guid ProductId { get; set; }

        public virtual Tag Tag { get; set; }

        public Guid TagId { get; set; }
    }
}
