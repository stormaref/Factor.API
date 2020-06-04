﻿using Factor.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models.RequestModels
{

    public class SubmitUserFactorRequestModel
    {
        [Required]
        [RegularExpression(StaticTools.PhoneRegex,ErrorMessage = StaticTools.PhoneValidationError)]
        public string UserPhone { get; set; }
        [Required]
        public List<FactorItem> FactorItems { get; set; }
        [Required]
        public string PreFactorId { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        public string ContactId { get; set; }
        [Required]
        public DateTime FactorDate { get; set; }
    }
}