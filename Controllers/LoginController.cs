using Factor.IServices;
using Factor.Models;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
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
            User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
            try
            {
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
                        _unitOfWork.VerificationRepository.Update(verification);
                        _unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        _unitOfWork.Rollback();
                        return Problem("Database error");
                    }
                }
                return Ok("Code sent");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public string TokenCheck()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims.ElementAt(1).Value.Split(' ').Last();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResendCode([FromQuery]string phone)
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

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCode([FromQuery]string phone, [FromQuery]long code)
        {
            var model = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.Phone == phone);
            if (model == null)
            {
                return BadRequest("Phone number is incorrect");
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
    }
}