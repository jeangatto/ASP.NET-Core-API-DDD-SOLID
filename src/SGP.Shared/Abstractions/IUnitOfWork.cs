using System;
using System.Threading;
using System.Threading.Tasks;

namespace SGP.Shared.Abstractions;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Salva todas as alterações feitas no contexto do banco de dados.
    /// </summary>
    /// <returns>O número de linhas afetadas no banco de dados.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}