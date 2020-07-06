using System;
using System.IO;
using System.Threading.Tasks;
using Factor.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ImageHandlerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ImageHandlerController> _logger;

        public ImageHandlerController(IUnitOfWork unitOfWork, ILogger<ImageHandlerController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetImage([FromQuery] string id)
        {
            try
            {
                var image = await _unitOfWork.ImageRepository.SingleOrDefaultAsync(i => i.Id == Guid.Parse(id));
                if (image == null)
                {
                    return NotFound();
                }
                var stream = new MemoryStream(image.Bytes);
                return File(stream, "image/jpeg");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message, e);
                return BadRequest();
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetImageRoute([FromRoute] string id)
        {
            try
            {
                var image = await _unitOfWork.ImageRepository.SingleOrDefaultAsync(i => i.Id == Guid.Parse(id));
                if (image == null)
                {
                    return NotFound();
                }
                var stream = new MemoryStream(image.Bytes);
                return File(stream, "image/jpeg");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message, e);
                return BadRequest();
            }
        }
    }
}
