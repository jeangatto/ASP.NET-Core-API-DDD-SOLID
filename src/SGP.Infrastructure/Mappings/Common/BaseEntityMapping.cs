using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Mappings.Common
{
    public abstract class BaseEntityMapping<TEntity> : GuidEntityKeyMapping<TEntity>
        where TEntity : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            // Configurando a coluna "CadastradoEm" como NOT NULL.
            builder.Property(e => e.CadastradoEm)
                .IsRequired()
                // Configurando para o banco nunca gerar o valor, a data é gerada pela aplicação.
                .ValueGeneratedNever()
                // Evitando que a coluna seja alterada após o INSERT.
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}