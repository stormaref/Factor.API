using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
