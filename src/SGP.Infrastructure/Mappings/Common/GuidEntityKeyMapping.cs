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
            // Definindo o nome da tabela pelo nome da classe.
            builder.ToTable(typeof(TEntity).Name);

            // Configurando a coluna "ID" como Chave Primária (PK).
            builder.HasKey(e => e.Id);

            // Configurando a coluna "ID" como NOT NULL.
            builder.Property(e => e.Id)
                .IsRequired()
                // Configurando para o banco nunca gerar o valor, o ID é gerado pela aplicação.
                .ValueGeneratedNever();
        }
    }
}
