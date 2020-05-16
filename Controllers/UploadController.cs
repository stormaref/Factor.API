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
        public IActionResult UploadFile([FromForm]ImageFile file)
        {
            if (file != null && file.File.ContentType == "image/jpeg")
            {
                var bytes = new byte[file.File.Length];
                file.File.OpenReadStream().Read(bytes, 0, bytes.Length);
                return Ok();
            }
            else
            {
                return BadRequest("File must be a jpeg imgae");
            }
        }
    }

    public class ImageFile
    {
        public IFormFile File { get; set; }
    }
}