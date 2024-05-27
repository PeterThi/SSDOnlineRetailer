using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using OnlineRetailer.Core;
using OnlineRetailer.CredentialsHandler;

namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<Customer> repository;

        public AdminController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        // GET: customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            MonitoringService.Log.Verbose("Following ip just recieved a list of all customers" + HttpContext.Connection.RemoteIpAddress.ToString());
            return repository.GetAll();

        }
    }
}
