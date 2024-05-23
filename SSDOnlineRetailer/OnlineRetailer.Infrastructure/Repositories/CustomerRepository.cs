using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly OnlineRetailerContext db;

        public CustomerRepository(OnlineRetailerContext context)
        {
            db = context;
        }

        public void Add(Customer entity)
        {
            db.Customer.Add(entity);
        }

        public void Edit(Customer entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public Customer Get(int id)
        {
            return db.Customer.Find(id);

        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customer.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
