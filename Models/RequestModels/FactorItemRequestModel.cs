using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{
    public class FactorItemRequestModel
    {
        [Required]
        public virtual ProductRequestModel Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long Price { get; set; }
    }
}
