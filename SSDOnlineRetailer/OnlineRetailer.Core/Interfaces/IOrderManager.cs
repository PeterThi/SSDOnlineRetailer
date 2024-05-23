using OnlineRetailer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.Core.Interfaces
{
    public interface IOrderManager
    {
        bool CreateOrder(Order order);
        bool UpdateOrder(Order order);
        bool CheckAvailability(Product product, OrderLine orderLine);
    }
}
