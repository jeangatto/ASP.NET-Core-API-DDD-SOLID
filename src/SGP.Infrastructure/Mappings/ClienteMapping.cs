using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Mappings.Common;

namespace SGP.Infrastructure.Mappings
{
    public class ClienteMapping : BaseEntityMapping<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Cliente));
        }
    }
}
