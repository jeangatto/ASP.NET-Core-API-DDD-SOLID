using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IUsuarioRepository : IAsyncRepository<Usuario>
    {
        Task<bool> EmailAlreadyExistsAsync(string email);
        Task<bool> EmailAlreadyExistsAsync(string email, Guid existingId);
        Task<Usuario> GetByEmailAsync(string email, string senha);
    }
}