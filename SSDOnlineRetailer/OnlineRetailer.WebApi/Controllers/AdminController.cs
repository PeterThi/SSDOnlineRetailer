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
        private LoginThrottler loginThrottler;
        public AdminController(IRepository<Customer> repos, LoginThrottler loginThrottler)
        {
            repository = repos;
            this.loginThrottler = loginThrottler;
        }

        // GET: customers
        [HttpGet]
        //[Authorize]
        public IActionResult Get()
        {
            if (User.IsInRole("Admin"))
            {
                MonitoringService.Log.Verbose("Following ip just recieved a list of all customers" + HttpContext.Connection.RemoteIpAddress.ToString());
                return Ok();
            }
            else
            {
                MonitoringService.Log.Warning("Following ip just tried an admin action " + HttpContext.Connection.RemoteIpAddress.ToString());
                return Unauthorized("Admins only");
            }
            
        }

        [HttpPatch]
        public void Patch(string ip) {
            loginThrottler.unbanIp(ip);
        }
    }
}
