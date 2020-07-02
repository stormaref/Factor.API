using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Factor.IServices;
using Factor.Migrations;
using Factor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Factor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ILogger<AdministratorController> _logger;

        public ProjectsController(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<AdministratorController> logger, IAuthService authService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddProject([FromQuery] string title)
        {
            try
            {
                if (await CheckProjectTitle(title))
                    return BadRequest();
                var user = await _authService.GetUser(HttpContext);
                if (user == null)
                    return NotFound("user not found");
                Project project = new Project(title, user.Id);
                try
                {
                    _unitOfWork.ProjectRepository.Insert(project);
                    _unitOfWork.Commit();
                    return Ok(project);
                }
                catch (Exception e)
                {
                    _unitOfWork.Rollback();
                    _logger.Log(LogLevel.Error, e.Message, e, project);
                    return Problem(e.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var user = await _unitOfWork.UserRepository.DbSet.Include(u => u.Projects).SingleOrDefaultAsync(u => Guid.Parse((HttpContext.User.Identity as ClaimsIdentity).Claims.Select(c => c.Value).ToList()[0]) == u.Id);
                if (user == null)
                    return NotFound("user not found");
                return Ok(user.Projects);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        private async Task<bool> CheckProjectTitle(string title)
        {
            var project = await _unitOfWork.ProjectRepository.DbSet.SingleOrDefaultAsync(p => p.Title == title.Trim() || p.Title == title);
            return project != null;
        }
    }
}
