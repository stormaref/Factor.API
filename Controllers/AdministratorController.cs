using Factor.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Factor.Models;

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
            if (model.Username == "admin" && model.Password == _configuration.GetValue<string>("AP"))
            {
                return Accepted("Login granted");
            }
            else
            {
                return Unauthorized("Username or password is incorrect");
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