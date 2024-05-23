using Microsoft.AspNetCore.Mvc;
using Core;
using Core.Entities;
namespace OnlineRetailerAPI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        // GET: customers
        [HttpGet]
        public IEnumerable<Customer> GetAll()
        {
            return repository.GetAll();
        }
    }
}
