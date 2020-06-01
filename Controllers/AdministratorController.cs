using Factor.IServices;
using Factor.Models;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMessageService _messageService;

        public AdministratorController(IMessageService messageService, IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<AdministratorController> logger)
        {
            _messageService = messageService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminLogin([FromQuery] string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {
                    bool check = _configuration.GetSection("AdminPhones").Get<string[]>().Any(s => s == phone);
                    if (check)
                    {
                        return await AdminLoginHandler(phone);
                    }
                    else
                    {
                        return Unauthorized("access denied");
                    }
                }
                else
                {
                    return BadRequest(StaticTools.PhoneValidationError);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        private async Task<IActionResult> AdminLoginHandler([FromQuery]string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {
                    User user = await _unitOfWork.UserRepository.GetDbContext().SingleOrDefaultAsync(u => u.Phone == phone);
                    long code = StaticTools.GenerateCode();
                    string response = await _messageService.SendSMS(phone, code);
                    if (user == null)
                    {
                        try
                        {
                            SMSVerification verification = new SMSVerification(code, phone);
                            User newUser = new User(phone)
                            {
                                Role = "Admin"
                            };
                            _unitOfWork.UserRepository.Insert(newUser);
                            verification.User = newUser;
                            _unitOfWork.VerificationRepository.Insert(verification);
                            _unitOfWork.Commit();
                            return Ok("Code sent");
                        }
                        catch (Exception)
                        {
                            _unitOfWork.Rollback();
                            return Problem("Database error");
                        }
                    }
                    else
                    {
                        SMSVerification verification = await _unitOfWork.VerificationRepository.GetDbContext().SingleOrDefaultAsync(v => v.User.Phone == phone);
                        try
                        {
                            verification.Code = code;
                            user.Role = "Admin";
                            _unitOfWork.VerificationRepository.Update(verification);
                            _unitOfWork.UserRepository.Update(user);
                            _unitOfWork.Commit();
                            return Ok("Code sent");
                        }
                        catch (Exception)
                        {
                            _unitOfWork.Rollback();
                            return Problem("Database error");
                        }
                    }
                }
                else
                {
                    return BadRequest(StaticTools.PhoneValidationError);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllFactors()
        {
            try
            {
                return Ok(_unitOfWork.FactorRepository.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserFactors([FromQuery]string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {
                    if (!_unitOfWork.UserRepository.GetDbContext().Any(u => u.Phone == phone))
                    {
                        return NotFound("User not found");
                    }
                    return Ok(_unitOfWork.FactorRepository.GetDbContext().Where(f => f.User.Phone == phone).OrderBy(f => f.UploadTime).AsEnumerable());
                }
                else
                {
                    return BadRequest(StaticTools.PhoneValidationError);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFactor([FromQuery]string id)
        {
            try
            {
                FactorItem factor = await _unitOfWork.FactorRepository.GetDbContext().SingleOrDefaultAsync(f => f.Id.ToString() == id);
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
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_unitOfWork.UserRepository.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }
    }
}