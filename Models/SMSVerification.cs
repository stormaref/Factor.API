using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class SMSVerification : BaseEntity
    {
        public long Code { get; set; }
        public string Phone { get; set; }
        public User User { get; set; }

        public SMSVerification(long code, string phone)
        {
            Code = code;
            Phone = phone;      
        }
    }
}
