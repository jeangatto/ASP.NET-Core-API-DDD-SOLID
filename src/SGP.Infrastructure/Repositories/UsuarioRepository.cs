using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories.Common;
using System;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SgpContext context)
            : base(context)
        {
        }

        public Task<bool> EmailAlreadyExistsAsync(string email)
        {
            return DbSet.AsNoTracking().AnyAsync(u => u.Email == email);
        }

        public Task<bool> EmailAlreadyExistsAsync(string email, Guid existingId)
        {
            return DbSet.AsNoTracking().AnyAsync(u => u.Email == email && u.Id != existingId);
        }

        public Task<Usuario> GetByEmailAsync(string email, string senha)
        {
            return DbSet.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }
    }
}