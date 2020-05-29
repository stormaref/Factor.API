using System;

namespace Factor.Models
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
