using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities.UserAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(SgpContext context)
            : base(context)
        {
        }

        public Task<bool> EmailAlreadyExistsAsync(Email email)
        {
            return GetQueryable()
                .AnyAsync(u => u.Email.Address == email.Address);
        }

        public Task<bool> EmailAlreadyExistsAsync(Email email, Guid existingId)
        {
            return GetQueryable()
                .AnyAsync(u => u.Email.Address == email.Address && u.Id != existingId);
        }

        public Task<User> GetByEmailAsync(Email email)
        {
            return GetQueryable(false)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email.Address == email.Address);
        }

        public Task<User> GetByTokenAsync(string refreshToken)
        {
            return GetQueryable()
                .Include(u => u.RefreshTokens.Any(t => t.Token == refreshToken))
                .FirstOrDefaultAsync();
        }
    }
}