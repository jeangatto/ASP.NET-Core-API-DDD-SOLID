using System;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Data.Repositories.Common;

public abstract class CachedRepositoryBase<TRepository> : IDisposable where TRepository : IRepository
{
    protected readonly ICacheService CacheService;
    protected readonly TRepository Repository;

    protected CachedRepositoryBase(ICacheService cacheService, TRepository repository)
    {
        CacheService = cacheService;
        Repository = repository;
    }

    #region IDisposable

    // To detect redundant calls.
    private bool _disposed;

    // Public implementation of Dispose pattern callable by consumers.
    ~CachedRepositoryBase()
    {
        Dispose(false);
    }

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        // Dispose managed state (managed objects).
        if (disposing)
            Repository.Dispose();

        _disposed = true;
    }

    #endregion
}