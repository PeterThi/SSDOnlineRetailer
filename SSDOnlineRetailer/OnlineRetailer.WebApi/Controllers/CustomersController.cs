using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;


namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        // GET: customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

    }
}
