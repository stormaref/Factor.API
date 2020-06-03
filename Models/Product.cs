using Factor.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Title { get; set; }
    }
}
