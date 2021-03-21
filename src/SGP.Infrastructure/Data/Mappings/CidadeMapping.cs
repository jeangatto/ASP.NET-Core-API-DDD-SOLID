using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Data.Mappings
{
    public class CidadeMapping : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ConfigureSingularTableName();

            builder.HasKey(cidade => cidade.Ibge);

            builder.Property(cidade => cidade.Ibge)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            builder.Property(cidade => cidade.Estado)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            builder.HasIndex(cidade => cidade.Estado);

            builder.Property(cidade => cidade.Nome)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);
        }
    }
}
