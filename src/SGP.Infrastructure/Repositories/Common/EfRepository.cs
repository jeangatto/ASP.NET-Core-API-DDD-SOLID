using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories.Common
{
    public abstract class EfRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        private readonly DbSet<TEntity> _dbSet;

        protected EfRepository(SgpContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void AddRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        public void Remove(TEntity entity) => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool readOnly = true)
        {
            return readOnly ?
                await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) :
                await _dbSet.FindAsync(id);
        }

        protected IQueryable<TEntity> Queryable(bool readOnly = true)
            => readOnly ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
    }
}