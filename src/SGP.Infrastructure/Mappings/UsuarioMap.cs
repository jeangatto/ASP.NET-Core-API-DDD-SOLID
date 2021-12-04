using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(usuario => usuario.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            // Mapeamento de Objetos de Valor (ValueObject)
            builder.OwnsOne(usuario => usuario.Email, ownedNav =>
            {
                ownedNav.Property(email => email.Address)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(100)
                    .HasColumnName(nameof(Usuario.Email));

                ownedNav.HasIndex(email => email.Address)
                    .IsUnique();
            });

            builder.Property(usuario => usuario.HashSenha)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(usuario => usuario.UltimoAcessoEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.BloqueioExpiraEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.NumeroFalhasAoAcessar)
                .IsRequired();

            builder.Navigation(usuario => usuario.Tokens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}