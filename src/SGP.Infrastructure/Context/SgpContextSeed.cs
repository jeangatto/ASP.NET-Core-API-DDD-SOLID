using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Shared.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Context
{
    public static class SgpContextSeed
    {
        public const string SeedFolderName = "Seeds";

        public static async Task EnsureSeedDataAsync(this SgpContext context)
        {
            Guard.Against.Null(context, nameof(context));

            if (!await context.Cidades.AsNoTracking().AnyAsync())
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), SeedFolderName, "Cidades.json");
                var cidadesAsJson = await File.ReadAllTextAsync(path, Encoding.UTF8);
                var cidadesAsDomain = cidadesAsJson.FromJson<IEnumerable<Cidade>>();
                context.AddRange(cidadesAsDomain);
                await context.SaveChangesAsync();
            }
        }
    }
}