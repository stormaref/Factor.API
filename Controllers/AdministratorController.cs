using Factor.IServices;
using Factor.Models;
using Factor.Models.RequestModels;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                    User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
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
                        SMSVerification verification = await _unitOfWork.VerificationRepository.GetDbSet().SingleOrDefaultAsync(v => v.User.Phone == phone);
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
        //[Authorize(Roles = "Admin")]
        public IActionResult GetAllUndoneFactors()
        {
            try
            {
                return Ok(_unitOfWork.PreFactorRepository.GetDbSet().Include(f=>f.Images).Where(f => !f.IsDone).OrderBy(f => f.CreationDate).ThenBy(f => f.User).AsAsyncEnumerable());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetFactorsWithTimeSpan([FromBody]TimeSpanRequestModel model)
        {
            try
            {
                return Ok(_unitOfWork.PreFactorRepository.Where(f => StaticTools.DateChecker(f.CreationDate, model.StartDate, model.EndDate)).OrderBy(f => f.User).ThenBy(f => f.CreationDate).AsAsyncEnumerable());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IsUserVerified([FromQuery]string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {

                    var user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
                    if (user == null)
                    {
                        return NotFound("user not found");
                    }
                    else
                    {
                        return Ok(user.IsVerified());
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

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserFactors([FromQuery]string phone)
        {
            try
            {
                if (StaticTools.PhoneValidator(phone))
                {
                    if (!_unitOfWork.UserRepository.GetDbSet().Any(u => u.Phone == phone))
                    {
                        return NotFound("User not found");
                    }
                    return Ok(_unitOfWork.PreFactorRepository.GetDbSet().Include(f=>f.Images).Where(f => f.User.Phone == phone).OrderBy(f => f.CreationDate).AsAsyncEnumerable());
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
                PreFactor factor = await _unitOfWork.PreFactorRepository.GetDbSet().Include(f=>f.Images).SingleOrDefaultAsync(f => f.Id.ToString() == id);
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

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubmitUserFactor([FromBody] SubmitUserFactorRequestModel model)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(u => u.Phone == model.UserPhone);
                if (user == null)
                    return NotFound("User not found");
                var factor = await _unitOfWork.PreFactorRepository.SingleOrDefaultAsync(f => f.Id.ToString() == model.PreFactorId);
                if (factor == null || factor.User.Id != user.Id)
                    return NotFound("pre-factor not found");
                var contact = user.Contacts.SingleOrDefault(c => c.Id.ToString() == model.ContactId);
                if (contact == null)
                    return NotFound("user contact not found");
                long totalPrice = 0;
                foreach (var item in model.FactorItems)
                {
                    item.TotalPrice = item.Quantity * item.Price;
                    totalPrice += item.TotalPrice;
                }
                var sf = new SubmittedFactor
                {
                    Contact = contact,
                    FactorDate = model.FactorDate,
                    TotalPrice = totalPrice,
                    Items = model.FactorItems,
                    PreFactor = factor,
                    State = model.State
                };
                try
                {
                    _unitOfWork.SubmittedFactorRepository.Insert(sf);
                    _unitOfWork.Commit();
                    //var x = new { Id = sf.Id, Items = sf.Items, PreFactor = sf.PreFactor, State = sf.State, TotalPrice = sf.TotalPrice };
                    return Ok(sf);
                }
                catch (Exception e)
                {
                    _unitOfWork.Rollback();
                    _logger.LogError(e, e.Message);
                    return Problem("database error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddContactToUser([FromBody]AddContactToUserRequestModel model)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(u => u.Phone == model.UserPhone);
                if (user == null)
                    return NotFound("User not found");
                if (_unitOfWork.ContactRepository.GetDbSet().Any(c => c.Name == model.ContactName))
                    return BadRequest("this name is available");
                var contact = new Contact(model.ContactName)
                {
                    User = user
                };
                try
                {
                    _unitOfWork.ContactRepository.Insert(contact);
                    _unitOfWork.Commit();
                    var x = new { Id = contact.Id, contact.Name };
                    return Ok(x);
                }
                catch (Exception e)
                {
                    _unitOfWork.Rollback();
                    _logger.LogError(e, e.Message);
                    return Problem("database error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserContacts(string phone)
        {
            try
            {
                var included = _unitOfWork.UserRepository.GetDbSet().Include(u => u.Contacts);
                var user = await included.SingleOrDefaultAsync(u => u.Phone == phone);
                var contacts = user.Contacts.ToList();
                var x = from contact in contacts
                        select new
                        {
                            Id = contact.Id,
                            Name = contact.Name,
                            UserId = contact.User.Id
                        };
                return Ok(x);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProducts([FromQuery]string search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) || string.IsNullOrEmpty(search))
                {
                    return Ok(_unitOfWork.ProductRepository.GetAll());
                }
                else
                {
                    return Ok(await _unitOfWork.ProductRepository.Where(p => p.Title.Contains(search)).ToListAsync());
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveProduct([FromQuery]string productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.SingleOrDefaultAsync(p => p.Id.ToString() == productId);
                if (product == null)
                    return NotFound("Product not found");
                try
                {
                    _unitOfWork.ProductRepository.Delete(Guid.Parse(productId));
                    _unitOfWork.Commit();
                    return Ok("Product removed succesfully");
                }
                catch (Exception e)
                {
                    _unitOfWork.Rollback();
                    _logger.LogError(e, e.Message);
                    return Problem("database error");
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