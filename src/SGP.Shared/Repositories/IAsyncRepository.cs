using SGP.Shared.Interfaces;

namespace SGP.Shared.Repositories
{
    /// <summary>
    /// Repositório Assíncrono.
    /// </summary>
    /// <typeparam name="TEntity">O tipo de entidade que será manipulada no banco de dados.</typeparam>
    public interface IAsyncRepository<TEntity> : IWriteOnlyRepository<TEntity>, IAsyncReadOnlyRepository<TEntity>
        where TEntity : IAggregateRoot
    {
    }
}