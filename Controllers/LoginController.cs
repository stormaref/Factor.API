using Factor.IServices;
using Factor.Models;
using Factor.Models.RequestModels;
using Factor.Models.ResponseModels;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class LoginController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IAuthService _authService;
        private readonly ILogger<LoginController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginController(IMessageService messageService, IAuthService authService, ILogger<LoginController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _messageService = messageService;
            _authService = authService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromQuery]string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {
                    User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
                    long code = StaticTools.GenerateCode();
                    string response = await _messageService.SendSMS(phone, code);
                    if (user == null)
                    {
                        try
                        {
                            SMSVerification verification = new SMSVerification(code, phone);
                            User newUser = new User(phone);
                            bool check = _configuration.GetSection("AdminPhones").Get<string[]>().Any(s => s == phone);
                            if (check)
                            {
                                newUser.Role = "Admin";
                            }
                            else
                            {
                                newUser.Role = "User";
                            }
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
                        SMSVerification verification = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.User.Phone == phone);
                        try
                        {
                            verification.Code = code;
                            _unitOfWork.VerificationRepository.Update(verification);
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
        [Authorize]
        public async Task<IActionResult> TokenCheck()
        {
            try
            {
                ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
                string number = identity.Claims.ElementAt(1).Value.Split(' ').Last();
                System.Collections.Generic.IEnumerable<Claim> claims = identity.Claims;
                return Ok(await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == number));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendCode([FromQuery]string phone)
        {
            try
            {
                SMSVerification model = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.Phone == phone);
                if (model == null)
                {
                    return BadRequest("Phone is not regestred yet");
                }
                else
                {
                    long code = StaticTools.GenerateCode();
                    try
                    {
                        string response = await _messageService.SendSMS(phone, code);
                        model.Code = code;
                        _unitOfWork.VerificationRepository.Update(model);
                        _unitOfWork.Commit();
                        return Ok("Code sent");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequestModel requestModel)
        {
            try
            {
                SMSVerification model = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.Phone == requestModel.Phone);
                if (model == null)
                {
                    return NotFound("Phone number is incorrect");
                }
                else
                {
                    if (model.Code == requestModel.Code)
                    {
                        User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == model.Phone);
                        if (user == null)
                        {
                            _logger.Log(LogLevel.Error, new Exception("database error"), "cannot get user by phone", model);
                            return Problem("Database error");
                        }
                        try
                        {
                            model.IsVerified = true;
                            _unitOfWork.VerificationRepository.Update(model);
                            _unitOfWork.Commit();
                            return Ok(new TokenResponseModel(requestModel.Phone, _authService.CreateToken(user)));
                        }
                        catch (Exception e)
                        {
                            _unitOfWork.Rollback();
                            _logger.Log(LogLevel.Error, e.Message, e, model);
                            return Problem(e.Message);
                        }
                    }
                    else
                    {
                        return BadRequest("Code is incorrect");
                    }
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