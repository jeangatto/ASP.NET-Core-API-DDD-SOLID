using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SGP.Infrastructure.UoW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SgpContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(SgpContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Ocorreu um erro (concorrência) ao salvar as informações na base de dados.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao salvar as informações na base de dados.");
                throw;
            }
        }
    }
}
