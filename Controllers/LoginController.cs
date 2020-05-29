using Factor.IServices;
using Factor.Models;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private IUnitOfWork _unitOfWork;

        public LoginController(IMessageService messageService, IAuthService authService, ILogger<LoginController> logger, IUnitOfWork unitOfWork)
        {
            _messageService = messageService;
            _authService = authService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromQuery]string phone)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
                var code = StaticTools.GenerateCode();
                var response = await _messageService.SendSMS(phone, code);
                if (user == null)
                {
                    SMSVerification verification = new SMSVerification(code, phone);
                    User newUser = new User(phone);
                    try
                    {
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
                    var verification = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.User.Phone == phone);
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
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var number = identity.Claims.ElementAt(1).Value.Split(' ').Last();
                return Ok(await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == number));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendCode([FromQuery]string phone)
        {
            try
            {
                var model = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.Phone == phone);
                if (model == null)
                {
                    return BadRequest("Phone is not regestred yet");
                }
                else
                {
                    var code = StaticTools.GenerateCode();
                    try
                    {
                        var response = await _messageService.SendSMS(phone, code);
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
                return Problem();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCode([FromQuery]string phone, [FromQuery]long code)
        {
            try
            {
                var model = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.Phone == phone);
                if (model == null)
                {
                    return NotFound("Phone number is incorrect");
                }
                else
                {
                    if (model.Code == code)
                    {
                        var user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == model.Phone);
                        if (user == null)
                        {
                            _logger.Log(LogLevel.Error, new Exception("database error"), "cannot get user by phone", model);
                            return Problem("Database error");
                        }
                        return Ok(new TokenResponseModel(phone, _authService.CreateToken(user)));
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
                return Problem();
            }
        }
    }
}