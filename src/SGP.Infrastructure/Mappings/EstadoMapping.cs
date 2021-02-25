using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Domain.Enums;
using SGP.Infrastructure.Mappings.Common;

namespace SGP.Infrastructure.Mappings
{
    public class EstadoMapping : GuidEntityKeyMapping<Estado>
    {
        public override void Configure(EntityTypeBuilder<Estado> builder)
        {
            base.Configure(builder);

            builder.Property(estado => estado.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            builder.Property(estado => estado.Sigla)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2)
                .IsFixedLength();

            builder.HasIndex(estado => estado.Sigla)
                .IsUnique();

            builder.Property(estado => estado.Ibge)
                .IsRequired();

            builder.Property(estado => estado.Regiao)
                .IsRequired()
                .HasConversion<string>()
                .IsUnicode(false)
                .HasMaxLength(12);

            builder.HasData(
                new Estado("Acre", "AC", 12, Regiao.Norte),
                new Estado("Alagoas", "AL", 27, Regiao.Nordeste),
                new Estado("Amapá", "AP", 16, Regiao.Norte),
                new Estado("Amazonas", "AM", 13, Regiao.Norte),
                new Estado("Bahia", "BA", 29, Regiao.Nordeste),
                new Estado("Ceará", "CE", 23, Regiao.Nordeste),
                new Estado("Distrito Federal", "DF", 53, Regiao.CentroOeste),
                new Estado("Espírito Santo", "ES", 32, Regiao.Sudeste),
                new Estado("Goiás", "GO", 52, Regiao.CentroOeste),
                new Estado("Maranhão", "MA", 21, Regiao.Nordeste),
                new Estado("Mato Grosso do Sul", "MS", 50, Regiao.CentroOeste),
                new Estado("Mato Grosso", "MT", 51, Regiao.CentroOeste),
                new Estado("Minas Gerais", "MG", 31, Regiao.Sudeste),
                new Estado("Pará", "PA", 15, Regiao.Norte),
                new Estado("Paraíba", "PB", 25, Regiao.Nordeste),
                new Estado("Paraná", "PR", 41, Regiao.Sul),
                new Estado("Pernambuco", "PE", 26, Regiao.Nordeste),
                new Estado("Piauí", "PI", 22, Regiao.Nordeste),
                new Estado("Rio de Janeiro", "RJ", 33, Regiao.Sudeste),
                new Estado("Rio Grande do Norte", "RN", 24, Regiao.Nordeste),
                new Estado("Rio Grande do Sul", "RS", 43, Regiao.Sul),
                new Estado("Rondônia", "RO", 11, Regiao.Norte),
                new Estado("Roraima", "RR", 14, Regiao.Norte),
                new Estado("Santa Catarina", "SC", 42, Regiao.Sul),
                new Estado("São Paulo", "SP", 35, Regiao.Sudeste),
                new Estado("Sergipe", "SE", 28, Regiao.Nordeste),
                new Estado("Tocantins", "TO", 17, Regiao.Norte));
        }
    }
}