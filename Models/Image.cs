using Factor.Repositories;

namespace Factor.Models
{
    public class Image : BaseEntity
    {
        public byte[] Bytes { get; set; }
    }
}