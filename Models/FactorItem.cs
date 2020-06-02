using Factor.Repositories;

namespace Factor.Models
{
    public class FactorItem : BaseEntity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long TotalPrice { get; set; }
    }
}
