using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using SGP.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories.Common
{
    public abstract class EfRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity, IAggregateRoot
    {
        private readonly DbSet<TEntity> _dbSet;

        protected EfRepository(SgpContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public virtual Task<TEntity> GetByIdAsync(Guid id, bool @readonly = true)
        {
            return GetQueryable(@readonly).FirstOrDefaultAsync(e => e.Id == id);
        }

        protected IQueryable<TEntity> GetQueryable(bool @readonly = true)
        {
            return @readonly ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
        }
    }
}