using Factor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class Product : BaseEntity
    {
        public string Title { get; set; }
    }
}
