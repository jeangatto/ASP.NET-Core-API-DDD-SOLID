using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SGP.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder RemoveCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }

            return modelBuilder;
        }
    }
}
