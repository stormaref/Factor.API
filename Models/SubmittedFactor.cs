using Factor.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    public class SubmittedFactor : BaseEntity
    {
        public List<FactorItem> Items { get; set; }
        public long TotalPrice { get; set; }
        public string Code { get; set; }
        [Required]
        public Contact Contact { get; set; }
        [JsonIgnore]
        public PreFactor PreFactor { get; set; }
    }
}
