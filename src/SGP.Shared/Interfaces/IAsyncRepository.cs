using System;
using System.Threading.Tasks;

namespace SGP.Shared.Interfaces
{
    public interface IAsyncRepository<TEntity> : IRepository where TEntity : IAggregateRoot
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        /// <summary>
        /// Encontra uma entidade pelo o valor da chave primária (id) fornecido.
        /// </summary>
        /// <param name="id">O valor da chave primária da entidade a ser encontrada.</param>
        /// <param name="readOnly">Quando verdadeiro a entidade não será rastreada pelo rastreador de mudanças do contexto.</param>
        /// <returns>A entidade encontrada ou nula.</returns>
        Task<TEntity> GetByIdAsync(Guid id, bool readOnly = true);
    }
}