using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SGP.Infrastructure.Extensions
{
    public static class DbSetExtensions
    {
        public static IQueryable<TEntity> AsTracking<TEntity>(this DbSet<TEntity> dbSet, bool readOnly)
            where TEntity : class => readOnly ? dbSet.AsNoTracking() : dbSet.AsQueryable();
    }
}