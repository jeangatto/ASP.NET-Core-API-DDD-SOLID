using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SGP.Shared.Interfaces
{
    /// <summary>
    /// Repositório Assíncrono.
    /// </summary>
    /// <typeparam name="TEntity">O tipo de entidade que será manipulada no banco de dados.</typeparam>
    public interface IAsyncRepository<TEntity> : IRepository where TEntity : IAggregateRoot
    {
        #region Write Methods

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Salva as alterações feitas no banco de dados.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>O número de entradas gravadas no banco de dados.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion

        #region ReadOnly Methods

        /// <summary>
        /// Encontra uma entidade com o valor da chave primária fornecido.
        /// </summary>
        /// <param name="id">O valor da chave primária da entidade a ser encontrada.</param>
        /// <param name="readonly">Quando verdadeiro a entidade não será rastreada pelo rastreador de mudanças do contexto.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A entidade encontrada ou nula.</returns>
        Task<TEntity> GetByIdAsync(Guid id, bool @readonly = true, CancellationToken cancellationToken = default);

        #endregion
    }
}