using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Shared.Extensions;

namespace SGP.Infrastructure.Context;

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

        var rowsAffected = await SeedAsync<Regiao>(context, "regioes.json");
        rowsAffected += await SeedAsync<Estado>(context, "estados.json");
        rowsAffected += await SeedAsync<Cidade>(context, "cidades.json");
        return rowsAffected;
    }

    private static async Task<long> SeedAsync<T>(DbContext context, string fileName) where T : class
    {
        Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));

        var totalRows = await context.Set<T>().AsNoTracking().LongCountAsync();
        if (totalRows == 0)
        {
            context.AddRange(await GetEntitiesFromJsonAsync<T>(fileName));
            totalRows = await context.SaveChangesAsync();
        }

        return totalRows;
    }

    private static async Task<IEnumerable<T>> GetEntitiesFromJsonAsync<T>(string fileName) where T : class
    {
        var filePath = Path.Combine(FolderPath, fileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"O arquivo de seed '{filePath}' não foi encontrado.", fileName);

        var entitiesJson = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return entitiesJson.FromJson<IEnumerable<T>>();
    }
}