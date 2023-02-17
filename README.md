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

[.NET 7](https://docs.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-7) + [EF Core 7.0](https://docs.microsoft.com/pt-br/ef/core/what-is-new/ef-core-7.0/whatsnew) + JWT Bearer + OpenAPI (Swagger)

> Nota: projeto focado em **Back-End**

- RESTful API
- Banco de dados relacional: **SQL Server**
- Cache Distribuído: **Redis**
- Clean Architecture
- Princípios **S.O.L.I.D.**
- Conceitos de modelagem de software **DDD (Domain Driven Design)**
- Padrão Repository-Service (Repository-Service Pattern)
- Padrão decorador (decorator pattern) [The decorator pattern](https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/)
- Padrão de Camada-Anticorrupção (Anti-Corruption Layer) **(FluentValidation)**
- Padrão Resultado **(FluentResults)** [Functional C#: Handling failures](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Scrutor](https://github.com/khellang/Scrutor) automaticamente registrando os serviços no ASP.NET Core DI
- Testes Unitários, Integrações com **xUnit**, **FluentAssertions**, **Moq**\
    => [Melhores práticas de teste de unidade com .NET Core](https://docs.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices)
- Monitoramento de performance da aplicação: [MiniProfiler for .NET](https://miniprofiler.com/dotnet/)
- Verificações de integridade da aplicação com [HealthChecks](https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)
- [SonarCloud](https://sonarcloud.io/project/overview?id=JeanGatto_SGP) para qualidade do código, codesmell, bugs, vulnerabilidades e cobertura de código

## Executando a aplicação usando o Docker

Após executar o comando no terminal `docker-compose up --build --abort-on-container-exit --remove-orphans`, basta abrir a url no navegador: `http://localhost:8000/swagger/`

## MiniProfiler for .NET

Para acessar a página com os indicadores de desempenho e performance:
`http://localhost:8000/profiler/results-index`

## Configurando Banco de dados

Por padrão é utilizado o SQL Server LocalDB, para alterar a conexão, modifique o valor da chave `DefaultConnection` no arquivo `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SgpContext;Trusted_Connection=True;MultipleActiveResultSets=true;",
    "Collation": "Latin1_General_CI_AI"
  }
}
```

Ao iniciar a aplicação o banco de dados será criado automaticamente e efetuado as migrações pendentenes,
também será populado o arquivo de seed.

```c#
await using var serviceScope = app.Services.CreateAsyncScope();
await using var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
var inMemoryOptions = serviceScope.ServiceProvider.GetOptions<InMemoryOptions>();

try
{
    app.Logger.LogInformation("----- AutoMapper: Validando os mapeamentos...");

    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    mapper.ConfigurationProvider.CompileMappings();

    app.Logger.LogInformation("----- AutoMapper: Mapeamentos são válidos!");

    if (inMemoryOptions.Cache)
    {
        app.Logger.LogInformation("----- Cache: InMemory");
    }
    else
    {
        app.Logger.LogInformation("----- Cache: Distributed");
    }

    if (inMemoryOptions.Database)
    {
        app.Logger.LogInformation("----- Database InMemory: Criando e migrando a base de dados...");
        await context.Database.EnsureCreatedAsync();
    }
    else
    {
        var connectionString = context.Database.GetConnectionString();
        app.Logger.LogInformation("----- SQL Server: {Connection}", connectionString);
        app.Logger.LogInformation("----- SQL Server: Verificando se existem migrações pendentes...");

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            app.Logger.LogInformation("----- SQL Server: Criando e migrando a base de dados...");

            await context.Database.MigrateAsync();

            app.Logger.LogInformation("----- SQL Server: Base de dados criada e migrada com sucesso!");
        }
        else
        {
            app.Logger.LogInformation("----- SQL Server: Migrações estão em dia.");
        }
    }

    app.Logger.LogInformation("----- Populando a base de dados...");

    await context.EnsureSeedDataAsync();

    app.Logger.LogInformation("----- Base de dados populada com sucesso!");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Ocorreu uma exceção ao iniciar a aplicação: {Message}", ex.Message);
    throw;
}

app.Logger.LogInformation("----- Iniciado a aplicação...");
app.Run();
```

## License

- [MIT License](https://github.com/JeanGatto/SGP/blob/main/LICENSE)
