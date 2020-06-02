using Factor.Repositories;
using System;

namespace Factor.Models
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public SubmittedFactor SubmittedFactor { get; set; }
        public Guid SubmitedFactorId { get; set; }
    }
}