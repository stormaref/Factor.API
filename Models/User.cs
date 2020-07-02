using Factor.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public ICollection<PreFactor> PreFactors { get; set; }
        public SMSVerification Verification { get; set; }
        [Required]
        public string Role { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Project> Projects { get; set; }
        public User(string phone)
        {
            Phone = phone;
            CreationDate = DateTime.Now;
        }
    }
}
