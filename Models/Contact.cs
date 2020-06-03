using Factor.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public SubmittedFactor SubmittedFactor { get; set; }
        public Guid SubmitedFactorId { get; set; }
    }
}