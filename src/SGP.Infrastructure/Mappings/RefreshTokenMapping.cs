using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ConfigureSingularTableName();
            builder.ConfigureBaseEntity();

            builder.Property(token => token.UsuarioId)
                .IsRequired();

            builder.Property(token => token.Token)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2048);

            builder.Property(token => token.CreatedAt)
                .IsRequired();

            builder.Property(token => token.ExpireAt)
                .IsRequired();

            builder.Property(token => token.RevokedAt)
                .IsRequired(false);

            builder.Property(token => token.ReplacedByToken)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(2048);

            builder.HasOne(token => token.Usuario)
                .WithMany(usuario => usuario.RefreshTokens)
                .HasForeignKey(token => token.UsuarioId);
        }
    }
}