using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Shared.Entities;

namespace SGP.Shared.Interfaces;

public interface IAsyncRepository<TEntity> : IRepository where TEntity : BaseEntity, IAggregateRoot
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<TEntity> GetByIdAsync(Guid id, bool readOnly = false);
}