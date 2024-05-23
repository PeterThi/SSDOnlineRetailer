using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Core.Services;



namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> repository;
        private readonly IOrderManager orderManager;

        public OrdersController(IRepository<Order> repos, IOrderManager manager)
        {
            repository = repos;
            orderManager = manager;
        }

        // GET: orders
        [HttpGet(Name = "GetOrders")]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }
        // GET order/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            if (!orderManager.CreateOrder(order))
            {
                return BadRequest("Order creation failed.");
            }

            return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
        }

        // PUT orders/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (order == null || order.Id != id)
            {
                return BadRequest();
            }

            if (!orderManager.UpdateOrder(order))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE orders/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return NoContent();
        }

    }
}
