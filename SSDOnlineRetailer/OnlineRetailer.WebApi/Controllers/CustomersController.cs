using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.CredentialsHandler;
using Monitoring;
namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repository;
        private LoginThrottler loginThrottler;
        public CustomersController(IRepository<Customer> repos, LoginThrottler loginThrottler)
        {
            repository = repos;
            this.loginThrottler = loginThrottler;
        }

        // GET: customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            MonitoringService.Log.Verbose("Following ip just recieved a list of all customers" + HttpContext.Connection.RemoteIpAddress.ToString());
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
        public IActionResult validateUser(int id, string password) 
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();

            if (loginThrottler.IsBlocked(ip)){
                Console.WriteLine("is blocked");
                MonitoringService.Log.Warning("Following ip was blocked for too many attempts:" + ip);
                return StatusCode(429, "This IP Has too many recent login attempts. Try again later");
                
            }

            bool isUserValid = PasswordHelper.ValidateCustomer(id, password, repository);

            if (isUserValid)
            {
                loginThrottler.RegisterAttempt(ip, true);

                return Ok("You have logged in");
            }
            else
            {
                loginThrottler.RegisterAttempt(ip,false);
                return Unauthorized("Invalid credentials");
            }
        }

    }
}
