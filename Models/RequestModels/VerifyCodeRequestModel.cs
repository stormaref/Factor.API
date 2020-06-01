using Factor.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models.RequestModels
{
    public class VerifyCodeRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is empty")]
        [RegularExpression(StaticTools.PhoneRegex, ErrorMessage = "Phone number is invalid")]
        public string Phone { get; set; }
        [RegularExpression("^[0-9]{4,8}$", ErrorMessage = "Code lenght must be between 4 and 8")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is empty")]
        public long Code { get; set; }
    }
}
