using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;

namespace SGP.Infrastructure.Mappings
{
    public class CityMapping : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(city => city.Ibge);

            builder.Property(city => city.Ibge)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            builder.Property(city => city.StateAbbr)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            builder.HasIndex(city => city.StateAbbr);

            builder.Property(city => city.Name)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);
        }
    }
}
