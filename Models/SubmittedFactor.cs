using Factor.Repositories;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    public class SubmittedFactor : BaseEntity
    {
        public List<FactorItem> Items { get; set; }
        public long TotalPrice { get; set; }
        public string Code { get; set; }
        public Contact Contact { get; set; }
        [JsonIgnore]
        public PreFactor PreFactor { get; set; }
    }
}
