using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentNegotiationController : Controller
    {
        [HttpGet]
        [Produces("application/json", "application/xml")]
        public IActionResult Get()
        {
            var data1 = new { message = "Hello, world!" };
            var data = new User { UserId = 100 , UserName = "Hello, world!" };
            return Ok(data);
        }
    }
    public class User() { 
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
