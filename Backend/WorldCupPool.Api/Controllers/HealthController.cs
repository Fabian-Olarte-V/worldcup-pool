using Microsoft.AspNetCore.Mvc;

namespace WorldCupPool.Api.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Health");
        }
    }
}
