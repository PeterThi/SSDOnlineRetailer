using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;



namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
