﻿using Factor.IServices;
using Factor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IConfiguration _configuration;
        private readonly string url;

        public ReportController(ILogger<ReportController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            url = _configuration.GetValue<string>("url");
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUserFactors()
        {
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.PreFactors).ThenInclude(f => f.Images).SingleOrDefaultAsync(u => u.Id == Guid.Parse(id));
            IOrderedEnumerable<PreFactor> preFactors = user.PreFactors.OrderBy(f => f.CreationDate).ThenBy(f => f.IsDone);                       
            var x = from item in preFactors
                    select new
                    {
                        item.Id,
                        item.UserId,
                        Images = GetImages(item.Images),
                        item.IsDone,
                        item.SubmittedFactorId
                    };
            return Ok(x);
        }

        private List<string> GetImages(List<Image> images)
        {
            return (from image in images select string.Format("{0}/api/Upload/GetImage?id={1}", url, image.Id)).ToList();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUserFactorsPaged([FromQuery] int page, [FromQuery] int size)
        {
            if (page <= 0 || size <= 0)
                return BadRequest("Invalid pagination numbers");
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.PreFactors).ThenInclude(f => f.Images).SingleOrDefaultAsync(u => u.Id == Guid.Parse(id));
            var preFactors = user.PreFactors.Skip((page - 1) * size).Take(size).OrderBy(f => f.CreationDate).ThenBy(f => f.IsDone);
            var data = from item in preFactors
                       select new
                       {
                           item.Id,
                           item.UserId,
                           item.Images,
                           item.IsDone,
                           item.SubmittedFactorId
                       };
            var result = new { Data = data, TotalPages = CalculateTotalPages(size, user.PreFactors.Count) };
            return Ok(result);
        }

        private static int CalculateTotalPages(int size, int count)
        {
            var t = count / size;
            return (count % size == 0) ? t : t + 1;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserContacts()
        {
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.Contacts).SingleOrDefaultAsync(u => u.Id == Guid.Parse(id));
            return Ok(user.Contacts);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserSubmittedFactors()
        {
            string id = (HttpContext.User.Identity as ClaimsIdentity).Claims.ElementAt(0).Value.Split(' ').Last();
            User user = await _unitOfWork.UserRepository.GetDbSet().Include(u => u.PreFactors).ThenInclude(pf => pf.SubmittedFactor).ThenInclude(sf => sf.Contact).SingleOrDefaultAsync(u => u.Id == Guid.Parse(id));
            System.Collections.Generic.List<SubmittedFactor> sf = user.PreFactors.Select(pf => pf.SubmittedFactor).ToList();
            var x = from item in sf
                    select new
                    {
                        item.Id,
                        item.Items,
                        item.TotalPrice,
                        item.Code,
                        item.Contact,
                        item.FactorDate,
                        item.State
                    };
            return Ok(user.Contacts);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetImage(string id)
        {
            var image = await _unitOfWork.ImageRepository.SingleOrDefaultAsync(i => i.Id == Guid.Parse(id));
            var stream = new MemoryStream(image.Bytes);
            return base.File(stream, "image/jpeg");
        }
    }
}
