﻿using System;

namespace Factor.Models
{
    public class SMSVerification : BaseEntity
    {
        public long Code { get; set; }
        public string Phone { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public SMSVerification(long code, string phone)
        {
            Code = code;
            Phone = phone;
        }
    }
}
