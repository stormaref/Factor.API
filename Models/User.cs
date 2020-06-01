using Factor.Repositories;
using System.Collections.Generic;

namespace Factor.Models
{
    public class User : BaseEntity
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<FactorItem> Factors { get; set; }
        public SMSVerification Verification { get; set; }
        public string Role { get; set; }
        public User(string phone)
        {
            Phone = phone;
        }
    }
}
