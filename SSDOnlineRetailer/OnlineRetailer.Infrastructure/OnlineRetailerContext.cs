
using OnlineRetailer.Core;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core.Entities;

namespace OnlineRetailer.Infrastructure
{
    public class OnlineRetailerContext : DbContext
    {
        public OnlineRetailerContext(DbContextOptions<OnlineRetailerContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Product> Product { get; set; }

    }
}
