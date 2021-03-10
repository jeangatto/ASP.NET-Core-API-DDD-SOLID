using System.Threading;
using System.Threading.Tasks;

namespace SGP.Shared.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Salva as alterações feitas no banco de dados.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>O número de entradas gravadas no banco de dados.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
