using Factor.Tools;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{
    public class AddContactToUserRequestModel
    {
        [Required]
        [RegularExpression(StaticTools.PhoneRegex, ErrorMessage = StaticTools.PhoneValidationError)]
        public string UserPhone { get; set; }
        [Required]
        public string ContactName { get; set; }
    }
}