using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities.UserAggregate;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureSingularTableName();
            builder.ConfigureBaseEntity();

            builder.Property(user => user.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            // Mapeamento ValueObject
            builder.OwnsOne(user => user.Email, ownedNav =>
            {
                ownedNav.Property(email => email.Address)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(100)
                    .HasColumnName("Email");

                ownedNav.HasIndex(email => email.Address)
                    .IsUnique();
            });

            builder.Property(user => user.PasswordHash)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(user => user.LastAccessAt)
                .IsRequired(false);

            builder.Property(user => user.LockExpires)
                .IsRequired(false);

            builder.Property(user => user.FailuresNum)
                .IsRequired();

            builder.Navigation(user => user.RefreshTokens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}