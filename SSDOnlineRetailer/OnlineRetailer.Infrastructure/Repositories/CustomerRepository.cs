using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Infrastructure;
using Monitoring;
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
            db.SaveChanges();
        }

        public Customer Get(int id)
        {
            return db.Customer.Find(id);

        }

        public IEnumerable<Customer> GetAll()
        {
            MonitoringService.Log.Verbose("All customer data was retrieved at" + DateTime.Now.ToString());
            return db.Customer.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
