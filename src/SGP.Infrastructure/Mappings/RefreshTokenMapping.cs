using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities.UserAggregate;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ConfigureSingularTableName();
            builder.ConfigureBaseEntity();

            builder.Property(token => token.UserId)
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

            builder.HasOne(token => token.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(token => token.UserId);
        }
    }
}