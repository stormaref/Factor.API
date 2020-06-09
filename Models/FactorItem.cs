using Factor.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    public class FactorItem : BaseEntity
    {
        [Required]
        public virtual Product Product { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long Price { get; set; }
        public long TotalPrice { get; set; }
        public Guid? SubmittedFactorId { get; set; }
        [JsonIgnore]
        public virtual SubmittedFactor SubmittedFactor { get; set; }

        public FactorItem()
        {
            CreationDate = DateTime.Now;
        }
    }
}
