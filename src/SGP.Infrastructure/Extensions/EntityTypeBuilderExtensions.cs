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
            // Configurando a coluna "Id" como Chave Primária (PK).
            builder.HasKey(e => e.Id);

            // Configurando a coluna como NOT NULL.
            // Configurando para o banco nunca gerar o valor, o "Id" é gerado pela aplicação.
            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
