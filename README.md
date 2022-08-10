# Sistema Gerenciador de Pedidos (SGP)

![visitors](https://visitor-badge.laobi.icu/badge?page_id=jeangatto.sgp)
[![wakatime](https://wakatime.com/badge/github/JeanGatto/SGP.svg)](https://wakatime.com/badge/github/JeanGatto/SGP)
[![License](https://img.shields.io/github/license/JeanGatto/SGP.svg)](LICENSE)

[![Build](https://github.com/JeanGatto/SGP/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/dotnet.yml)
[![SonarCloud](https://github.com/JeanGatto/SGP/actions/workflows/sonar-cloud.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/sonar-cloud.yml)
[![CodeQL](https://github.com/JeanGatto/SGP/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/codeql-analysis.yml)
[![DevSkim](https://github.com/JeanGatto/SGP/actions/workflows/devskim-analysis.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/devskim-analysis.yml)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=JeanGatto_SGP)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=coverage)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=JeanGatto_SGP)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=bugs)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=code_smells)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)

Criado com o [Rider: o IDE .NET de plataforma cruzada da JetBrains](https://www.jetbrains.com/pt-br/rider/)

![Rider logo](https://resources.jetbrains.com/storage/products/company/brand/logos/Rider_icon.svg)

C# 10 + [.NET 6](https://docs.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-6) + [EF Core 6.0](https://docs.microsoft.com/pt-br/ef/core/what-is-new/ef-core-6.0/whatsnew) + JWT Bearer + OpenAPI (Swagger)

> Nota: projeto focado em **Back-End**

- RESTful API
- Clean Architecture
- Princípios **S.O.L.I.D.**
- Conceitos de modelagem de software **DDD (Domain Driven Design)**
- Padrão Repository-Service (Repository-Service Pattern)
- Padrão de Camada-Anticorrupção (Anti-Corruption Layer) **(FluentValidation)**
- Padrão Resultado **(FluentResults)** [Functional C#: Handling failures](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- Utilizando **Floating Versions** (wildcard) nos pacotes NuGet
- [Scrutor](https://github.com/khellang/Scrutor) automaticamente registrando os serviços no ASP.NET Core DI
- Testes Unitários, Integrações com **xUnit**, **FluentAssertions**, **Moq**\
    => [Melhores práticas de teste de unidade com .NET Core](https://docs.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices)
- Monitoramento de performance da aplicação: [MiniProfiler for .NET](https://miniprofiler.com/dotnet/)
- Verificações de integridade da aplicação com [HealthChecks](https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)
- [SonarCloud](https://sonarcloud.io/) para qualidade do código, codesmell, bugs, vulnerabilidades e cobertura de código

## Executando a aplicação usando o Docker

Após executar o comando no terminal `docker-compose up --build --abort-on-container-exit`, basta abrir a url no navegador: `http://localhost:8000/swagger/`

## MiniProfiler for .NET

Para acessar a página com os indicadores de desempenho e performance:
`http://localhost:8000/profiler/results-index`

## Configurando Banco de dados

Por padrão é utilizado o SQL Server LocalDB, para alterar a conexão, modifique o valor da chave `DefaultConnection` no arquivo `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SgpContext;Trusted_Connection=True;MultipleActiveResultSets=true;"
  }
}
```

Ao iniciar a aplicação o banco de dados será criado automaticamente e efetuado as migrações pendentenes,
também será populado o arquivo de seed.

```c#
public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();

    await using var scope = host.Services.CreateAsyncScope();
    await using var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

    try
    {
        logger.LogInformation("Database Connection: {ConnectionString}", context.Database.GetConnectionString());

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            logger.LogInformation("Creating and migrating the database...");
            await context.Database.MigrateAsync();
        }

        logger.LogInformation("Seeding database...");
        await context.EnsureSeedDataAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while populating the database");
        throw;
    }

    logger.LogInformation("Starting the application...");
    await host.RunAsync();
}
```

## License

- [MIT License](https://github.com/JeanGatto/SGP/blob/main/LICENSE)
