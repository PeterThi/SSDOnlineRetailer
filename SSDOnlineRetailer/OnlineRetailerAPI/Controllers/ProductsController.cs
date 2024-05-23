using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace OnlineRetailerAPI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> repository;

        public ProductsController(IRepository<Product> repos)
        {
            repository = repos;
        }

        // GET: products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return repository.GetAll();
        }
    }
}
