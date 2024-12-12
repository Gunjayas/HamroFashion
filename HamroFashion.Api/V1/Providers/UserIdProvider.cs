namespace HamroFashion.Api.V1.Providers
{
    public interface IUserIdProvider
    {
        public Guid? GetUserId();
    }

    public class StaticUserIdProvider : IUserIdProvider
    {
        private Guid? id { get; set; }

        public StaticUserIdProvider(Guid? id)
        {
            this.id = id;
        }

        public Guid? GetUserId()
        {
            return id;
        }
    }
}
