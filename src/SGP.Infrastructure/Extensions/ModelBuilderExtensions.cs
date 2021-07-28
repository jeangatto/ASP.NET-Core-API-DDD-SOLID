using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace SGP.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Remove a delação em cascata de chaves estrangeiras (FK).
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static ModelBuilder RemoveCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            Guard.Against.Null(modelBuilder, nameof(modelBuilder));

            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(entity => entity.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            return modelBuilder;
        }
    }
}