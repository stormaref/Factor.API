using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{
    public class StateRequestModel
    {
        [Required]
        public bool IsClear { get; set; }
    }
}
