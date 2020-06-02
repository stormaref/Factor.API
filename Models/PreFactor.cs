﻿using Factor.Repositories;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Factor.Models
{
    public class PreFactor : BaseEntity
    {
        public List<Image> Images { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public DateTime UploadTime { get; set; }
        public bool IsDone { get; set; }
        public string Description { get; set; }
        public SubmittedFactor SubmittedFactor { get; set; }
        public Guid SubmittedFactorId { get; set; }
        public PreFactor(DateTime uploadTime)
        {
            UploadTime = uploadTime;
        }
    }
}
