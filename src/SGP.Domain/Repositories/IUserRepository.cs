using SGP.Domain.Entities.UserAggregate;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<bool> EmailAlreadyExistsAsync(Email email);
        Task<bool> EmailAlreadyExistsAsync(Email email, Guid existingId);
        Task<User> GetByEmailAsync(Email email);
        Task<User> GetByTokenAsync(string refreshToken);
    }
}