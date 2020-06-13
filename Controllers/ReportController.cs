using Factor.IServices;
using Factor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(ILogger<ReportController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUserFactors()
        {
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.PreFactors).ThenInclude(f => f.Images).SingleOrDefaultAsync(u => u.Id.ToString() == id);
            var preFactors = user.PreFactors.OrderBy(f=>f.CreationDate).ThenBy(f=>f.IsDone);
            var x = from item in preFactors
                    select new
                    {
                        item.Id,
                        item.UserId,
                        item.Images,
                        item.IsDone,
                        item.SubmittedFactorId
                    };
            return Ok(x);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserContacts()
        {
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            Models.User user = await _unitOfWork.UserRepository.GetDbSet().SingleOrDefaultAsync(u => u.Id.ToString() == id);
            return Ok(user.Contacts);
        }
    }
}
