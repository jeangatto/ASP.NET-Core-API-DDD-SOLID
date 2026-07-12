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

## Visão geral

O **SGP** é um sistema gerenciador de pedidos desenvolvido com arquitetura limpa e princípios de DDD, SOLID e boas práticas de testes. O projeto demonstra uma API RESTful em **ASP.NET Core 10** com integração de SQL Server, Redis e observabilidade.

## Recursos principais

- API RESTful construída com **ASP.NET Core 10**
- Camadas separadas: `SGP.PublicApi`, `SGP.Application`, `SGP.Domain`, `SGP.Infrastructure`, `SGP.Shared`
- Persistência com **Entity Framework Core 10** e **SQL Server**
- Cache distribuído com **Redis**
- Injeção de dependência com **Scrutor** e padrões de design como Decorator
- Validação de entrada com **FluentValidation**
- Tratamento funcional de erros com **FluentResults**
- Monitoramento com **MiniProfiler for .NET**
- Health checks para verificação de integridade
- Testes unitários e de integração com **xUnit**, **FluentAssertions** e **NSubstitute**
- Integração contínua e análise de qualidade com **GitHub Actions** e **SonarCloud**

## Pré-requisitos

- .NET SDK 10
- Docker e Docker Compose (opcional para execução em contêiner)
- SQL Server LocalDB ou outra instância SQL Server configurada

## Executando localmente

1. Abra o terminal na pasta do repositório.
2. Restaure os pacotes:

```powershell
dotnet restore SGP.slnx
```

3. Execute a API:

```powershell
dotnet run --project src\SGP.PublicApi\SGP.PublicApi.csproj
```

4. Acesse a documentação Swagger:

```text
http://localhost:8000/swagger/
```

## Executando com Docker

Para rodar a aplicação em contêineres Docker:

```powershell
docker-compose up --build
```

Em seguida, abra:

```text
http://localhost:8000/swagger/
```

### MiniProfiler

Acesse os dados de performance em:

```text
http://localhost:8000/profiler/results-index
```

## Configuração de banco de dados

Por padrão, a aplicação utiliza o `DefaultConnection` definido em `src\SGP.PublicApi\appsettings.Development.json`.

Exemplo de configuração:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SgpContext;Trusted_Connection=True;MultipleActiveResultSets=true;",
    "Collation": "Latin1_General_CI_AI"
  }
}
```

> Se você usar SQL Server em contêiner, atualize a string de conexão conforme necessário.

## Testes

Execute os testes com:

```powershell
dotnet test SGP.slnx
```

## Estrutura do repositório

- `src/SGP.PublicApi/` - aplicação Web API e pontos de entrada
- `src/SGP.Application/` - lógica de aplicação e casos de uso
- `src/SGP.Domain/` - entidades de domínio, agregados e regras de negócio
- `src/SGP.Infrastructure/` - persistência, caches e integrações externas
- `src/SGP.Shared/` - abstrações compartilhadas, validações e constantes
- `tests/SGP.Tests/` - testes automatizados

## Contribuição

Contribuições são bem-vindas! Se quiser colaborar:

1. Crie uma issue descrevendo a mudança.
2. Abra um pull request com pequenas alterações.
3. Verifique se todos os testes passam antes de submeter.

## Licença

Licenciado sob a [MIT License](LICENSE).
