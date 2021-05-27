using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories.Common
{
    public abstract class EfRepository<TEntity> : IAsyncRepository<TEntity>
       where TEntity : BaseEntity, IAggregateRoot
    {
        private readonly DbSet<TEntity> _dbSet;

        protected EfRepository(SgpContext context) => _dbSet = context.Set<TEntity>();

        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void Remove(TEntity entity) => _dbSet.Remove(entity);

        public virtual Task<TEntity> GetByIdAsync(Guid id, bool readOnly = true)
            => Queryable(readOnly).FirstOrDefaultAsync(e => e.Id == id);

        protected IQueryable<TEntity> Queryable(bool readOnly = true)
            => readOnly ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
    }
}