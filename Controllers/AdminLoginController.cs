using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromQuery] string Username, [FromQuery] string Password)
        {
            if (Username == "admin" && Password == _configuration.GetValue<string>("AP"))
            {
                return Accepted();
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }

        [HttpGet("[action]")]
        public async void GetAllFactors()
        {

        }
    }
}