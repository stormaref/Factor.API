using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{
    public class ImageFilesRequestModel
    {
        [Required]
        public List<IFormFile> Files { get; set; }
        public string Description { get; set; }
    }
}
