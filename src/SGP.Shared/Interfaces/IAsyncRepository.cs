using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Shared.Interfaces
{
    public interface IAsyncRepository<TEntity> : IRepository where TEntity : IAggregateRoot
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Encontra uma entidade pelo o valor da chave primária fornecido.
        /// </summary>
        /// <param name="id">O valor da chave primária da entidade a ser encontrada.</param>
        /// <param name="readonly">Quando verdadeiro a entidade não será rastreada pelo rastreador de mudanças do contexto.</param>
        /// <returns>A entidade encontrada ou nula.</returns>
        Task<TEntity> GetByIdAsync(Guid id, bool @readonly = true);
    }
}