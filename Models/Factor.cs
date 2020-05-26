using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class Factor : BaseEntity
    {
        public byte[] Image { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public DateTime UploadTime { get; set; }
        public bool IsDone { get; set; }

        public Factor(byte[] image, string userId, DateTime uploadTime)
        {
            Image = image;
            UserId = userId;
            UploadTime = uploadTime;
        }
    }
}
