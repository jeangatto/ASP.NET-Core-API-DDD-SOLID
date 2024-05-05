using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Data;

public sealed class UnitOfWork(SgpContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var rowsAffected = await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("----- Row(s) affected: {RowsAffected}", rowsAffected);

            return rowsAffected;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "Ocorreu um erro (concorrência) ao salvar as informações na base de dados");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao salvar as informações na base de dados");
            throw;
        }
    }

    #region IDisposable

    // To detect redundant calls.
    private bool _disposed;

    // Public implementation of Dispose pattern callable by consumers.
    ~UnitOfWork() => Dispose(false);

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        // Dispose managed state (managed objects).
        if (disposing)
            context.Dispose();

        _disposed = true;
    }

    #endregion
}