using System.Collections.Generic;

namespace OnlineRetailer.Core
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T entity);
        void Edit(T entity);
        void Remove(int id);
    }
}
