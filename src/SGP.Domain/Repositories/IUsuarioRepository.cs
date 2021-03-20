using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Shared.Repositories;
using System;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IUsuarioRepository : IAsyncRepository<Usuario>
    {
        Task<bool> EmailAlreadyExistsAsync(Email email);
        Task<bool> EmailAlreadyExistsAsync(Email email, Guid existingId);
        Task<Usuario> GetByEmailAsync(Email email);
    }
}