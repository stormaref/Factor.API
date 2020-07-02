using Factor.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Factor.IRepositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(Guid id);
        DbSet<T> GetDbSet();
        DbSet<T> DbSet { get; }
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
