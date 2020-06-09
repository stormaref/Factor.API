using Factor.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public Product(string title)
        {
            Title = title;
            CreationDate = DateTime.Now;
        }
    }
}
