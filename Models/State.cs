using Factor.Repositories;
using System;

namespace Factor.Models
{
    public class State : BaseEntity
    {
        public bool IsClear { get; set; }

        public State(bool isClear)
        {
            IsClear = isClear;
            CreationDate = DateTime.Now;
        }
    }
}
