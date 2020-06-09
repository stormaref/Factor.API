using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models.RequestModels
{
    public class StateRequestModel
    {
        [Required]
        public bool IsClear { get; set; }
    }
}
