using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGP.Domain.Entities;
using SGP.Shared.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Data.Context
{
    /// <summary>
    /// Responsável por popular a base de dados.
    /// </summary>
    public static class SgpContextSeed
    {
        /// <summary>
        /// Nome da pasta que contém os arquivos físicos do seed.
        /// </summary>
        public const string SeedFolderName = "Seeds";

        /// <summary>
        /// Caminho da pasta raiz da aplicação.
        /// </summary>
        private static readonly string RootFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Popula a base de dados.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public static async Task EnsureSeedDataAsync(this SgpContext context, ILoggerFactory loggerFactory)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(loggerFactory, nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger(nameof(SgpContextSeed));

            if (!await context.Cidades.AsNoTracking().AnyAsync())
            {
                var path = Path.Combine(RootFolderPath, SeedFolderName, "Cidades.json");
                if (!File.Exists(path))
                {
                    logger.LogError($"O arquivo de seed '{path}' não foi encontradao.");
                }
                else
                {
                    var cidadesAsJson = await File.ReadAllTextAsync(path, Encoding.UTF8);
                    var cidadesAsDomain = cidadesAsJson.FromJson<IEnumerable<Cidade>>();
                    context.AddRange(cidadesAsDomain);
                    var rowsAffected = await context.SaveChangesAsync();
                    logger.LogInformation($"Total de cidades inseridas: {rowsAffected}");
                }
            }
        }
    }
}