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

namespace SGP.Infrastructure.Context
{
    /// <summary>
    /// Responsável por popular a base de dados.
    /// </summary>
    public static class SgpContextSeed
    {
        /// <summary>
        /// Caminho da pasta raiz da aplicação.
        /// </summary>
        private static readonly string RootFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Caminho da pasta que contém os arquivos de seeds.
        /// </summary>
        private static readonly string FolderPath = Path.Combine(RootFolderPath, "Seeds");

        /// <summary>
        /// Popula a base de dados.
        /// </summary>
        /// <param name="context">Contexto da base de dados.</param>
        /// <param name="loggerFactory"></param>
        /// <returns>Retorna o número de linhas afetadas na base de dados.</returns>
        public static async Task<long> EnsureSeedDataAsync(this SgpContext context, ILoggerFactory loggerFactory)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(loggerFactory, nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger(nameof(SgpContextSeed));
            var rowsAffected = await PopularAsync<Regiao>(context, logger, "regioes.json");
            rowsAffected += await PopularAsync<Estado>(context, logger, "estados.json");
            rowsAffected += await PopularAsync<Cidade>(context, logger, "cidades.json");
            return rowsAffected;
        }

        private static async Task<int> PopularAsync<T>(SgpContext context, ILogger logger, string jsonFileName) where T : class
        {
            Guard.Against.Null(logger, nameof(logger));
            Guard.Against.Null(jsonFileName, nameof(jsonFileName));

            var rowsAffected = 0;

            var dbSet = context.Set<T>();
            if (!await dbSet.AsNoTracking().AnyAsync())
            {
                var filePath = Path.Combine(FolderPath, jsonFileName);
                if (!File.Exists(filePath))
                {
                    logger.LogError($"O arquivo '{filePath}' não foi encontradao.");
                }
                else
                {
                    var entitiesJson = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                    dbSet.AddRange(entitiesJson.FromJson<IEnumerable<T>>());

                    rowsAffected = await context.SaveChangesAsync();
                    logger.LogInformation($"Total de '{rowsAffected}' registros inseridos em '{typeof(T).Name}'.");
                }
            }

            return rowsAffected;
        }
    }
}