using Factor.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<PreFactor> PreFactors { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        public Project(string title, Guid userId)
        {
            Title = title;
            UserId = userId;
        }
    }
}
