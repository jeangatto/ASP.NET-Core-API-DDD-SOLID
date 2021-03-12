using SGP.Shared.Interfaces;

namespace SGP.Shared.Repositories
{
    public interface IAsyncRepository<TEntity> : IWriteOnlyRepository<TEntity>,
        IAsyncReadOnlyRepository<TEntity> where TEntity : IAggregateRoot
    {
    }
}