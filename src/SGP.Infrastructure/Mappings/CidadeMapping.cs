using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Mappings.Common;

namespace SGP.Infrastructure.Mappings
{
    public class CidadeMapping : GuidEntityKeyMapping<Cidade>
    {
        public override void Configure(EntityTypeBuilder<Cidade> builder)
        {
            base.Configure(builder);

            builder.Property(cidade => cidade.EstadoId)
                .IsRequired();

            builder.Property(cidade => cidade.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(70);

            builder.Property(cidade => cidade.Ibge)
                .IsRequired();

            builder.HasOne(cidade => cidade.Estado)
                .WithMany(estado => estado.Cidades)
                .HasForeignKey(cidade => cidade.EstadoId);
        }
    }
}
