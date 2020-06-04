using Factor.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    [Table("PreFactor")]
    public class PreFactor : BaseEntity
    {
        public List<Image> Images { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [Required]
        public bool IsDone { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual SubmittedFactor SubmittedFactor { get; set; }
        public Guid? SubmittedFactorId { get; set; }
        public PreFactor()
        {
            CreationDate = DateTime.Now;
        }
    }
}
