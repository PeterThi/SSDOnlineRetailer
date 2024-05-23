using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Infrastructure.Repositories;

namespace OnlineRetailer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineRetailerContext _context;
        private IRepository<Order> _orders;
        private IRepository<Product> _products;
        private IRepository<Customer> _customers;
        private bool _disposed = false;

        public UnitOfWork(OnlineRetailerContext context)
        {
            _context = context;
        }

        public IRepository<Order> Orders => _orders ??= new OrderRepository(_context);
        public IRepository<Product> Products => _products ??= new ProductRepository(_context);
        public IRepository<Customer> Customers => _customers ??= new CustomerRepository(_context);

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
