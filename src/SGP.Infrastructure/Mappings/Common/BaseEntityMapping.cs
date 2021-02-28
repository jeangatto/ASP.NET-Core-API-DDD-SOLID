using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Mappings.Common
{
    public abstract class BaseEntityMapping<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(typeof(TEntity).Name);

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