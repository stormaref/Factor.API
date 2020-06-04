using Factor.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace Factor.Models
{
    public class State : BaseEntity
    {
        [Required]
        public bool IsClear { get; set; }

        public State(bool isClear)
        {
            IsClear = isClear;
            CreationDate = DateTime.Now;
        }
    }
}
