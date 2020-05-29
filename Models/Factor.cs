using System;

namespace Factor.Models
{
    public class Factor : BaseEntity
    {
        public byte[] Image { get; set; }
        public virtual User User { get; set; }
        public DateTime UploadTime { get; set; }
        public bool IsDone { get; set; }

        public Factor(byte[] image, DateTime uploadTime)
        {
            Image = image;
            UploadTime = uploadTime;
        }
    }
}
