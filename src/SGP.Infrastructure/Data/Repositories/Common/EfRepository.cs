using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Data.Repositories.Common;

public abstract class EfRepository<TEntity>(SgpContext context)
    : RepositoryBase<TEntity>(context), IAsyncRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
{
    public void Add(TEntity entity) =>
        DbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) =>
        DbSet.AddRange(entities);

    public void Update(TEntity entity) =>
        DbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) =>
        DbSet.UpdateRange(entities);

    public void Remove(TEntity entity) =>
        DbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) =>
        DbSet.RemoveRange(entities);

    public virtual async Task<TEntity> GetByIdAsync(Guid id, bool readOnly = false) =>
        readOnly ? await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) : await DbSet.FindAsync(id);
}