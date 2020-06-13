using Factor.IServices;
using Factor.Models;
using Factor.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
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

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> UploadFiles([FromForm] ImageFilesRequestModel model)
        {
            try
            {
                if (CheckFiles(model.Files))
                {
                    List<Image> vs = new List<Image>();
                    foreach (IFormFile file in model.Files)
                    {
                        byte[] bytes = new byte[file.Length];
                        file.OpenReadStream().Read(bytes, 0, bytes.Length);
                        vs.Add(new Image { Bytes = bytes });
                    }
                    ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
                    string id = identity.Claims.ElementAt(0).Value.Split(' ').Last();
                    PreFactor factor = new PreFactor()
                    {
                        Images = vs,
                        User = await _authService.GetUser(id),
                        Description = model.Description
                    };
                    try
                    {
                        _unitOfWork.PreFactorRepository.Insert(factor);
                        _unitOfWork.Commit();
                        return Ok(factor);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error, ex, "transaction rollback", factor);
                        _unitOfWork.Rollback();
                        return Problem("database rollback");
                    }
                }
                else
                {
                    return BadRequest("All images must be JPEG");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        private bool CheckFiles(List<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                if (file == null || file.ContentType != "image/jpeg")
                {
                    return false;
                }
            }
            return true;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async IAsyncEnumerable<FileContentResult> GetPreFactorImages([FromQuery] string factorId)
        {
            PreFactor factor = await _unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.Images).SingleOrDefaultAsync(f => f.Id.ToString() == factorId);
            foreach (Image image in factor.Images)
            {
                yield return File(image.Bytes, "image/jpeg");
            }
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> AddImagesToFactor([FromBody] AddImageToFactorRequestModel model)
        {
            try
            {
                PreFactor factor = await _unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.Images).SingleOrDefaultAsync(f => f.Id == Guid.Parse(model.FactorId));
                if (factor == null)
                {
                    return NotFound("factor not found");
                }

                if (CheckFiles(model.Images))
                {
                    List<Image> vs = new List<Image>();
                    foreach (IFormFile file in model.Images)
                    {
                        byte[] bytes = new byte[file.Length];
                        file.OpenReadStream().Read(bytes, 0, bytes.Length);
                        vs.Add(new Image(bytes));
                    }
                    factor.Images.AddRange(vs);
                    try
                    {
                        _unitOfWork.PreFactorRepository.Update(factor);
                        _unitOfWork.Commit();
                        return Ok(factor.Images);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error, ex, "transaction rollback", factor);
                        _unitOfWork.Rollback();
                        return Problem("database rollback");
                    }
                }
                else
                {
                    return BadRequest("All images must be JPEG");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }
    }


}