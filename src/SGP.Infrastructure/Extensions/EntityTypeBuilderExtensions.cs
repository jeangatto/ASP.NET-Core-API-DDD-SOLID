using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configuração da Entidade Base <see cref="BaseEntity" />.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
