using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class Factor : BaseEntity
    {
        public byte[] Image { get; set; }
        public User User { get; set; }
        public DateTime UploadTime { get; set; }
        public bool IsDone { get; set; }

        public Factor(byte[] image, User user, DateTime uploadTime)
        {
            Image = image;
            User = user;
            UploadTime = uploadTime;
        }
    }
}
