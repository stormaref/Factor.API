using Factor.IServices;
using Factor.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class AdministratorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdministratorController> _logger;

        public AdministratorController(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<AdministratorController> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] AdminLoginRequestModel model)
        {
            try
            {
                if (model.Username == "admin" && model.Password == _configuration.GetValue<string>("AP"))
                {
                    return Accepted("Login granted");
                }
                else
                {
                    return Unauthorized("Username or password is incorrect");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetAllFactors()
        {
            try
            {
                return Ok(_unitOfWork.FactorRepository.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetUserFactors([FromQuery]string phone)
        {
            try
            {
                if (!_unitOfWork.UserRepository.GetDbSet().Any(u => u.Phone == phone))
                {
                    return NotFound("User not found");
                }
                return Ok(_unitOfWork.FactorRepository.GetDbSet().Where(f => f.User.Phone == phone).OrderBy(f => f.UploadTime).AsEnumerable());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFactor([FromQuery]string id)
        {
            try
            {
                var factor = await _unitOfWork.FactorRepository.GetDbSet().SingleOrDefaultAsync(f => f.Id.ToString() == id);
                if (factor != null)
                {
                    return Ok(factor);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_unitOfWork.UserRepository.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }
    }
}