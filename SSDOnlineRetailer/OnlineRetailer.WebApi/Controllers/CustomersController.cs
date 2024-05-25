using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.CredentialsHandler;

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

        [HttpPut]

        public void UpdatePassword(int id, string password)
        {
            string passwordHash = PasswordHelper.HashPassword(password);
            Customer customerToUpdate = repository.Get(id);
            customerToUpdate.hashedPassword = passwordHash;

            repository.Edit(customerToUpdate);
        }

        [HttpGet("{id}, {password}")]
        public bool validateUser(int id, string password) 
        {
            return PasswordHelper.ValidateCustomer(id, password, repository);
        }

    }
}
