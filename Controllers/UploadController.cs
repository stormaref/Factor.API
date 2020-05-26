using Factor.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public UploadController(ILogger<UploadController> logger, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile([FromForm]ImageFile file)
        {
            if (file != null && file.File.ContentType == "image/jpeg")
            {
                var bytes = new byte[file.File.Length];
                file.File.OpenReadStream().Read(bytes, 0, bytes.Length);
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var id = identity.Claims.ElementAt(0).Value.Split(' ').Last();
                var factor = new Models.Factor(bytes, id,DateTime.Now);
                factor.User = await _authService.GetUser(id);
                try
                {
                    _unitOfWork.FactorRepository.Insert(factor);
                    _unitOfWork.Commit();
                    return Ok(factor);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "transaction rollback", factor);
                    _unitOfWork.Rollback();
                    return Problem();
                }
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