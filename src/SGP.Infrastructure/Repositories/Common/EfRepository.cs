using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Repositories.Common
{
    public abstract class EfRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        protected readonly DbSet<TEntity> DbSet;

        protected EfRepository(SgpContext context)
            => DbSet = context.Set<TEntity>();

        public void Add(TEntity entity)
            => DbSet.Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => DbSet.AddRange(entities);

        public void Update(TEntity entity)
            => DbSet.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities)
            => DbSet.UpdateRange(entities);

        public void Remove(TEntity entity)
            => DbSet.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => DbSet.RemoveRange(entities);

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool readOnly = true)
            => readOnly ? await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) : await DbSet.FindAsync(id);
    }
}