using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly OnlineRetailerContext db;

        public ProductRepository(OnlineRetailerContext context)
        {
            db = context;
        }

        public void Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Product entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public Product Get(int id)
        {
            return db.Product.Find(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Product.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
