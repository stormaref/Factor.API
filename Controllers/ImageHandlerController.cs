using System;
using System.IO;
using System.Threading.Tasks;
using Factor.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ImageHandlerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageHandlerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                return BadRequest();
            }
        }
    }
}
