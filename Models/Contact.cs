using Factor.Repositories;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual SubmittedFactor SubmittedFactor { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public Contact(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
        }
    }
}