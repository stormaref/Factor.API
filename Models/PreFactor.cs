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
        [Required]
        public string Title { get; set; }
        public List<Image> Images { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public bool IsDone { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual SubmittedFactor SubmittedFactor { get; set; }
        public Guid? SubmittedFactorId { get; set; }
        [JsonIgnore]
        public virtual Project Project { get; set; }
        public Guid? ProjectId { get; set; }
        public PreFactor()
        {
            CreationDate = DateTime.Now;
        }
    }
}
