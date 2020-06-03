using Factor.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class FactorItem : BaseEntity
    {
        [Required]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long TotalPrice { get; set; }
    }
}
