using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly SgpContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(SgpContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var rowsAffected = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Row(s) affected: {RowsAffected}", rowsAffected);
            return rowsAffected;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro (concorrência) ao salvar as informações na base de dados");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao salvar as informações na base de dados");
            throw;
        }
    }

    #region IDisposable

    // To detect redundant calls.
    private bool _disposed;

    // Public implementation of Dispose pattern callable by consumers.
    ~UnitOfWork()
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
            _context.Dispose();

        _disposed = true;
    }

    #endregion
}