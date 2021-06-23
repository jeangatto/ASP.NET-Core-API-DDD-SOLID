using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class TokenAcessoMapeamento : IEntityTypeConfiguration<TokenAcesso>
    {
        public void Configure(EntityTypeBuilder<TokenAcesso> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(token => token.UsuarioId)
                .IsRequired();

            builder.Property(token => token.Token)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2048);

            builder.Property(token => token.CriadoEm)
                .IsRequired();

            builder.Property(token => token.ExpiraEm)
                .IsRequired();

            builder.Property(token => token.RevogadoEm)
                .IsRequired(false);

            builder.HasOne(token => token.Usuario)
                .WithMany(usuario => usuario.Tokens)
                .HasForeignKey(token => token.UsuarioId);
        }
    }
}
