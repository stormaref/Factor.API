using Factor.IRepositories;
using System;

namespace Factor.Repositories
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
