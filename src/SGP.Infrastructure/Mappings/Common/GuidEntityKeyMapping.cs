using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Mappings.Common
{
    public abstract class GuidEntityKeyMapping<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : GuidEntityKey
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(typeof(TEntity).Name);

            builder.HasKey(e => e.Id); // Configurando a coluna "Id" como Chave Primária (PK).

            builder.Property(e => e.Id)
                .IsRequired()           // Configurando a coluna como NOT NULL.
                .ValueGeneratedNever(); // Configurando para o banco nunca gerar o valor, o "Id" é gerado pela aplicação.
        }
    }
}
