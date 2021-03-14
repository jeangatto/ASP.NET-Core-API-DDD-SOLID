using SGP.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGP.Shared.Repositories
{
    public interface IAsyncReadOnlyRepository<TEntity> where TEntity : IAggregateRoot
    {
        /// <summary>
        /// Encontra uma entidade pelo o valor da chave primária fornecido.
        /// </summary>
        /// <param name="id">O valor da chave primária da entidade a ser encontrada.</param>
        /// <param name="readonly">Quando verdadeiro a entidade não será rastreada pelo rastreador de mudanças do contexto.</param>
        /// <returns>A entidade encontrada ou nula.</returns>
        Task<TEntity> GetByIdAsync(Guid id, bool @readonly = true);
    }
}