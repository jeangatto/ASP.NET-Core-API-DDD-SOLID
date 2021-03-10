using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Enums;
using SGP.Infrastructure.Context;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Popula a base de dados com as informações essenciais (básicas) para o funcionamento do sistema.
        /// </summary>
        /// <param name="context"></param>
        public static async Task EnsureSeedDataAsync(this SgpContext context)
        {
            Guard.Against.Null(context, nameof(context));

            if (!await context.Paises.AsNoTracking().AnyAsync())
            {
                context.Paises.Add(new Pais("Brasil", "BR", 1058));
                await context.SaveChangesAsync();
            }

            if (!context.Estados.AsNoTracking().Any())
            {
                var paisBrasil = await context.Paises.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Sigla == "BR");

                Guard.Against.Null(paisBrasil, nameof(paisBrasil));

                context.Estados.AddRange(new Estado[]
                {
                    new Estado(paisBrasil.Id, "Acre", "AC", 12, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Alagoas", "AL", 27, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Amapá", "AP", 16, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Amazonas", "AM", 13, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Bahia", "BA", 29, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Ceará", "CE", 23, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Distrito Federal", "DF", 53, Regiao.CentroOeste),
                    new Estado(paisBrasil.Id, "Espírito Santo", "ES", 32, Regiao.Sudeste),
                    new Estado(paisBrasil.Id, "Goiás", "GO", 52, Regiao.CentroOeste),
                    new Estado(paisBrasil.Id, "Maranhão", "MA", 21, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Mato Grosso do Sul", "MS", 50, Regiao.CentroOeste),
                    new Estado(paisBrasil.Id, "Mato Grosso", "MT", 51, Regiao.CentroOeste),
                    new Estado(paisBrasil.Id, "Minas Gerais", "MG", 31, Regiao.Sudeste),
                    new Estado(paisBrasil.Id, "Pará", "PA", 15, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Paraíba", "PB", 25, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Paraná", "PR", 41, Regiao.Sul),
                    new Estado(paisBrasil.Id, "Pernambuco", "PE", 26, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Piauí", "PI", 22, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Rio de Janeiro", "RJ", 33, Regiao.Sudeste),
                    new Estado(paisBrasil.Id, "Rio Grande do Norte", "RN", 24, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Rio Grande do Sul", "RS", 43, Regiao.Sul),
                    new Estado(paisBrasil.Id, "Rondônia", "RO", 11, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Roraima", "RR", 14, Regiao.Norte),
                    new Estado(paisBrasil.Id, "Santa Catarina", "SC", 42, Regiao.Sul),
                    new Estado(paisBrasil.Id, "São Paulo", "SP", 35, Regiao.Sudeste),
                    new Estado(paisBrasil.Id, "Sergipe", "SE", 28, Regiao.Nordeste),
                    new Estado(paisBrasil.Id, "Tocantins", "TO", 17, Regiao.Norte)
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
