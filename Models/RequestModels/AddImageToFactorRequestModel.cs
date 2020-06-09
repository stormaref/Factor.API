using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Factor.Models.RequestModels
{
    public class AddImageToFactorRequestModel
    {
        public List<IFormFile> Images { get; set; }
        public string FactorId { get; set; }
    }
}
