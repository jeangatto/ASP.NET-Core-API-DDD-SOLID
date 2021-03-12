using SGP.Shared.Interfaces;
using System.Collections.Generic;

namespace SGP.Shared.Repositories
{
    public interface IWriteOnlyRepository<in TEntity> where TEntity : IAggregateRoot
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}