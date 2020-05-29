﻿using System.Collections.Generic;

namespace Factor.Models
{
    public class User : BaseEntity
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Factor> Factors { get; set; }
        public SMSVerification Verification { get; set; }
        public User(string phone)
        {
            Phone = phone;
        }
    }
}
