using Factor.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class Image : BaseEntity
    {
        [Required]
        public byte[] Bytes { get; set; }
    }
}