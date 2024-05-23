using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure
{
    public interface IDbInitializer
    {
        void Initialize(OnlineRetailerContext context);
    }
}
