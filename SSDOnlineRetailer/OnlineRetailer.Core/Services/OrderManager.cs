using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using System.Transactions;
using Monitoring;
namespace OnlineRetailer.Core.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateOrder(Order order)
        {
            if (order == null)
            {
                MonitoringService.Log.Error("null order found");
                throw new ArgumentNullException(nameof(order));
                
            }
            if (order.OrderLines == null || !order.OrderLines.Any()) throw new ArgumentException("Order must have at least one order line.", nameof(order.OrderLines));

            try
            {
                Customer customer = _unitOfWork.Customers.Get(order.CustomerId) ?? throw new InvalidOperationException("Customer not found");

                decimal totalCost = CalculateTotalCost(order);

                if (totalCost >= 0 )
                {
                    MonitoringService.Log.Error("Inappropriate order found with cost >= 0 with order id: " + order.Id);
                }

                if (customer.Balance < totalCost)
                {
                    MonitoringService.Log.Error("User with inelligble balance tried to order with id: " + order.Id);
                    return false;

                }

                UpdateProductStocks(order);

                customer.Balance -= totalCost;
                _unitOfWork.Customers.Edit(customer);

                order.TotalPrice = totalCost;
                _unitOfWork.Orders.Add(order);

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return false;
            }
            
        }

        //this isnt implemented yet. Could do next ig
        public bool UpdateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            return false;
        }

        public bool CheckAvailability(Product product, OrderLine orderLine)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (orderLine == null) throw new ArgumentNullException(nameof(orderLine));

            return product.ItemsInStock >= orderLine.Quantity;
        }
        private decimal CalculateTotalCost(Order order)
        {
            decimal totalCost = 0;

            foreach (var orderLine in order.OrderLines)
            {
                Product product = _unitOfWork.Products.Get(orderLine.ProductId) ?? throw new InvalidOperationException("Product not found");

                if (!CheckAvailability(product, orderLine))
                {
                    throw new InvalidOperationException($"Product {product.Id} is out of stock.");
                }

                totalCost += orderLine.Quantity * product.Price;
            }

            return totalCost;
        }

        private void UpdateProductStocks(Order order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                Product product = _unitOfWork.Products.Get(orderLine.ProductId);
                product.ItemsInStock -= orderLine.Quantity;
                _unitOfWork.Products.Edit(product);
            }
        }


    }
}

