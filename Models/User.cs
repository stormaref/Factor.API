using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class User : BaseEntity
    {
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
