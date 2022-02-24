using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Shared.Extensions;

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
        /// Nome da pasta que contém os arquivos de seeds.
        /// </summary>
        private const string SeedFolderName = "Seeds";

        /// <summary>
        /// Caminho da pasta que contém os arquivos de seeds.
        /// </summary>
        private static readonly string FolderPath = Path.Combine(RootFolderPath, SeedFolderName);

        /// <summary>
        /// Popula a base de dados.
        /// </summary>
        /// <param name="context">Contexto da base de dados.</param>
        /// <returns>Retorna o número de linhas afetadas na base de dados.</returns>
        public static async Task<long> EnsureSeedDataAsync(this SgpContext context)
        {
            Guard.Against.Null(context, nameof(context));

            var rowsAffected = await PopularAsync<Regiao>(context, "regioes.json");
            rowsAffected += await PopularAsync<Estado>(context, "estados.json");
            rowsAffected += await PopularAsync<Cidade>(context, "cidades.json");
            return rowsAffected;
        }

        private static async Task<long> PopularAsync<TEntity>(DbContext context, string jsonFileName) where TEntity : class
        {
            Guard.Against.NullOrWhiteSpace(jsonFileName, nameof(jsonFileName));

            var dbSet = context.Set<TEntity>();

            var totalRows = await dbSet.AsNoTracking().LongCountAsync();
            if (totalRows == 0)
            {
                var filePath = Path.Combine(FolderPath, jsonFileName);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"O arquivo de seed '{filePath}' não foi encontrado.", jsonFileName);

                var entitiesJson = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                dbSet.AddRange(entitiesJson.FromJson<IEnumerable<TEntity>>());

                totalRows = await context.SaveChangesAsync();
            }

            return totalRows;
        }
    }
}