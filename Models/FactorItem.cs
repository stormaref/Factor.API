using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    public class FactorItem : BaseEntity
    {
        public List<Image> Images { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public DateTime UploadTime { get; set; }
        public bool IsDone { get; set; }

        public FactorItem(DateTime uploadTime)
        {
            UploadTime = uploadTime;
        }
    }
}
