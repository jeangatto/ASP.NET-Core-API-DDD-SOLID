using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories.Common
{
    public abstract class EfRepository<TEntity> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity, IAggregateRoot
    {
        #region Constructor

        protected readonly DbSet<TEntity> DbSet;
        private readonly SGPContext _context;

        protected EfRepository(SGPContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        #endregion

        #region Write Methods

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region ReadOnly Methods

        public virtual Task<TEntity> GetByIdAsync(Guid id, bool @readonly = true, CancellationToken cancellationToken = default)
        {
            return GetQueryable(@readonly).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        protected IQueryable<TEntity> GetQueryable(bool @readonly)
        {
            return @readonly ? DbSet.AsNoTracking() : DbSet;
        }

        #endregion
    }
}