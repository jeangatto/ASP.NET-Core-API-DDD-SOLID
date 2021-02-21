namespace SGP.Shared.Interfaces
{
    public interface IAsyncRepository<TEntity> : IRepository where TEntity : class, IAggregateRoot
    {
    }
}