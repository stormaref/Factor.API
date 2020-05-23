using Factor.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Login([FromQuery] string Username, [FromQuery] string Password)
        {
            if (Username == "admin" && Password == _configuration.GetValue<string>("AP"))
            {
                return Accepted("Login granted");
            }
            else
            {
                return BadRequest("Username or password is incorrect");
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
    }
}