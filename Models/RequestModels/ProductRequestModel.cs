using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{
    public class ProductRequestModel
    {
        [Required]
        public string Title { get; set; }
    }
}