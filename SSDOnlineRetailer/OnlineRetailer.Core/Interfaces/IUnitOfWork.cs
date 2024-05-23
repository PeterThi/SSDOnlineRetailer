using OnlineRetailer.Core.Entities;

namespace OnlineRetailer.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Order> Orders { get; }
        IRepository<Product> Products { get; }
        IRepository<Customer> Customers { get; }
        void Commit();
        void Rollback();
    }
}
