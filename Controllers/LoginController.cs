using Factor.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMessageService _service;

        public LoginController(IMessageService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SendSMS(string phone)
        {
            try
            {
                var response = await _service.SendSMS(phone, GenerateCode());
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private long GenerateCode()
        {
            Random random = new Random();
            return random.Next(1111, 9999);
        }
    }
}