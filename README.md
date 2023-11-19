# Sistema Gerenciador de Pedidos (SGP)

[![Build](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/dotnet.yml)
[![SonarCloud](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/sonar-cloud.yml/badge.svg)](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/sonar-cloud.yml)
[![CodeQL](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/codeql-analysis.yml)
[![DevSkim](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/devskim-analysis.yml/badge.svg)](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/actions/workflows/devskim-analysis.yml)
[![License](https://img.shields.io/github/license/JeanGatto/ASP.NET-Core-API-DDD-SOLID.svg)](LICENSE)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=ASP.NET-Core-API-DDD-SOLID)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=coverage)](https://sonarcloud.io/dashboard?id=ASP.NET-Core-API-DDD-SOLID)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=ASP.NET-Core-API-DDD-SOLID)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=ASP.NET-Core-API-DDD-SOLID)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=ASP.NET-Core-API-DDD-SOLID)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=bugs)](https://sonarcloud.io/dashboard?id=ASP.NET-Core-API-DDD-SOLID)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=ASP.NET-Core-API-DDD-SOLID&metric=code_smells)](https://sonarcloud.io/dashboard?id=ASP.NET-Core-API-DDD-SOLID)

## Dê uma estrela! ⭐

Se você gostou deste projeto, aprendeu algo, dê uma estrelinha. Obrigado!

- ASP.NET Core 8
- Entity Framework Core 8
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
- Testes Unitários, Integrações com **xUnit**, **FluentAssertions**, **NSubstitute**\
   => [Melhores práticas de teste de unidade com .NET Core](https://docs.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices)
- Monitoramento de performance da aplicação: [MiniProfiler for .NET](https://miniprofiler.com/dotnet/)
- Verificações de integridade da aplicação com [HealthChecks](https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)
- [SonarCloud](https://sonarcloud.io/project/overview?id=ASP.NET-Core-API-DDD-SOLID) para qualidade do código, codesmell, bugs, vulnerabilidades e cobertura de código

## Executando a aplicação usando o Docker

Após executar o comando no terminal `docker-compose up --build`, basta abrir a url no navegador: `http://localhost:8000/swagger/`

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

## License

- [MIT License](https://github.com/JeanGatto/ASP.NET-Core-API-DDD-SOLID/blob/main/LICENSE)
