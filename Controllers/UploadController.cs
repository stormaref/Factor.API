using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult UploadFile([FromForm(Name = "file")]IFormFile file)
        {
            if (file != null && file.ContentType == "image/jpeg")
            {
                var bytes = new byte[file.Length];
                file.OpenReadStream().Read(bytes, 0, bytes.Length);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}