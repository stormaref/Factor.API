using System;

namespace Factor.IRepositories
{
    internal interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
