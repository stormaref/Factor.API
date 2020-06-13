using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models.RequestModels
{

    public class SubmitUserFactorRequestModel
    {
        [Required]
        public List<FactorItemRequestModel> FactorItems { get; set; }
        [Required]
        public string PreFactorId { get; set; }
        [Required]
        public StateRequestModel State { get; set; }
        [Required]
        public string ContactId { get; set; }
        [Required]
        public DateTime FactorDate { get; set; }
    }
}
