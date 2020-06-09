using Factor.IServices;
using Factor.Models;
using Factor.Models.RequestModels;
using Factor.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUndoneFactors()
        {
            try
            {
                return Ok(_unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.Images).Where(f => !f.IsDone).OrderBy(f => f.CreationDate).ThenBy(f => f.User).AsAsyncEnumerable());
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

                    User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Phone == phone);
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

                    IEnumerable<PreFactor> result = _unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.Images).Include(f => f.User).Where(f => f.User.Phone == phone).OrderBy(f => f.CreationDate).AsEnumerable();
                    var x = from item in result
                            select new
                            {
                                item.Id,
                                UserId = item.User.Id,
                                item.Images,
                                item.IsDone,
                                item.SubmittedFactorId
                            };
                    return Ok();
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
                PreFactor factor = await _unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.Images).SingleOrDefaultAsync(f => f.Id.ToString() == id);
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

        [HttpPost("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubmitUserFactor([FromBody] SubmitUserFactorRequestModel model)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.Contacts).SingleOrDefaultAsync(u => u.Phone == model.UserPhone);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                PreFactor preFactor = await _unitOfWork.PreFactorRepository.GetDbSet().Include(f => f.User).SingleOrDefaultAsync(f => f.Id.ToString() == model.PreFactorId);
                if (preFactor == null || preFactor.User.Id != user.Id)
                {
                    return NotFound("pre-factor not found");
                }

                Contact contact = user.Contacts.SingleOrDefault(c => c.Id.ToString() == model.ContactId);
                if (contact == null)
                {
                    return NotFound("user contact not found");
                }

                long totalPrice = 0;
                List<FactorItem> factors = new List<FactorItem>();
                foreach (FactorItemRequestModel item in model.FactorItems)
                {
                    Product product = await _unitOfWork.ProductRepository.SingleOrDefaultAsync(p => p.Title == item.Product.Title);
                    if (product == null)
                    {
                        return NotFound(item.Product.Title + " not found");
                    }

                    FactorItem factorItem = new FactorItem()
                    {
                        Price = item.Price,
                        Product = product,
                        TotalPrice = item.Quantity * item.Price
                    };
                    factors.Add(factorItem);
                    totalPrice += factorItem.TotalPrice;
                }
                SubmittedFactor sf = new SubmittedFactor
                {
                    Contact = contact,
                    FactorDate = model.FactorDate,
                    TotalPrice = totalPrice,
                    Items = factors,
                    PreFactor = preFactor,
                    State = new State(model.State.IsClear)
                };
                try
                {
                    _unitOfWork.SubmittedFactorRepository.Insert(sf);
                    _unitOfWork.Commit();
                    var x = new { Id = sf.Id, Items = sf.Items, State = sf.State, TotalPrice = sf.TotalPrice };
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

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddContactToUser([FromBody]AddContactToUserRequestModel model)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(u => u.Phone == model.UserPhone);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (_unitOfWork.ContactRepository.GetDbSet().Any(c => c.Name == model.ContactName))
                {
                    return BadRequest("this name is available");
                }

                Contact contact = new Contact(model.ContactName)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserContacts(string phone)
        {
            try
            {
                Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<User, ICollection<Contact>> included = _unitOfWork.UserRepository.GetDbSet().Include(u => u.Contacts);
                User user = await included.SingleOrDefaultAsync(u => u.Phone == phone);
                List<Contact> contacts = user.Contacts.ToList();
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
        [Authorize(Roles = "Admin")]
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

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromQuery]string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrEmpty(title))
                {
                    return BadRequest("Product tite is invalid");
                }

                Product test = await _unitOfWork.ProductRepository.SingleOrDefaultAsync(p => p.Title == title || p.Title == title.Trim());
                if (test != null)
                {
                    return BadRequest("There is a product with this title");
                }

                try
                {
                    _unitOfWork.ProductRepository.Insert(new Product(title));
                    _unitOfWork.Commit();
                    return Ok("Product added succesfully");
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


        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct([FromQuery]string productId)
        {
            try
            {
                Product product = await _unitOfWork.ProductRepository.SingleOrDefaultAsync(p => p.Id.ToString() == productId);
                if (product == null)
                {
                    return NotFound("Product not found");
                }

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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSubmittedFactorItems([FromQuery]string submittedFactorId)
        {
            try
            {
                SubmittedFactor sf = await _unitOfWork.SubmittedFactorRepository.GetDbSet().Include(s => s.Items).SingleOrDefaultAsync(s => s.Id == Guid.Parse(submittedFactorId));
                if (sf == null)
                {
                    return NotFound("Factor not found");
                }

                return Ok(sf.Items);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }
    }
}