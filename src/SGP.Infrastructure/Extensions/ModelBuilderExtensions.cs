using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SGP.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Remove a delação em cascata de chaves estrangeiras (FK).
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void RemoveCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(entity => entity.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}