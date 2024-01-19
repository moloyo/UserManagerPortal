using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_configuration.ToString());
        }

        [HttpGet("database")]
        public IActionResult GetDatabase()
        {
            return Ok(_configuration.GetConnectionString("UserDatabase"));
        }
    }
}
