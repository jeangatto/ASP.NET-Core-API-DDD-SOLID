using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            return modelBuilder;
        }
    }
}