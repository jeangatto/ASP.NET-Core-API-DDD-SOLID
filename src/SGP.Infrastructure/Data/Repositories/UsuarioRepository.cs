using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SgpContext context)
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

        public Task<Usuario> GetByEmailAsync(Email email)
        {
            return GetQueryable(false)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email.Address == email.Address);
        }

        public Task<Usuario> GetByTokenAsync(string refreshToken)
        {
            return GetQueryable()
                .Include(u => u.RefreshTokens.Any(t => t.Token == refreshToken))
                .FirstOrDefaultAsync();
        }
    }
}