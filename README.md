# SGP

###### Sistema Gerenciador de Pedidos

![visitors](https://visitor-badge.laobi.icu/badge?page_id=jeangatto.sgp)
[![wakatime](https://wakatime.com/badge/github/JeanGatto/SGP.svg)](https://wakatime.com/badge/github/JeanGatto/SGP)
[![License](https://img.shields.io/github/license/JeanGatto/SGP.svg)](LICENSE)

[![Build](https://github.com/JeanGatto/SGP/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/dotnet.yml)
[![SonarCloud](https://github.com/JeanGatto/SGP/actions/workflows/sonar-cloud.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/sonar-cloud.yml)
[![CodeQL](https://github.com/JeanGatto/SGP/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/codeql-analysis.yml)
[![DevSkim](https://github.com/JeanGatto/SGP/actions/workflows/devskim-analysis.yml/badge.svg)](https://github.com/JeanGatto/SGP/actions/workflows/devskim-analysis.yml)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=coverage)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=bugs)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=JeanGatto_SGP&metric=code_smells)](https://sonarcloud.io/dashboard?id=JeanGatto_SGP)

C# 10 + [.NET 6](https://docs.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-6) + [EF Core 6.0](https://docs.microsoft.com/pt-br/ef/core/what-is-new/ef-core-6.0/whatsnew) + JWT Bearer + OpenAPI (Swagger)

> Nota: projeto focado em **Back-End**

- RESTful API + GraphQL
- Aplicado os princípios do **S.O.L.I.D.**
- Aplicado a abordagem de modelagem de software **DDD (Domain Driven Design)**
- Padrão de Camada Anticorrupção **(FluentValidation)**
- Padrão Resultado **(FluentResults)** [Functional C#: Handling failures](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Scrutor](https://github.com/khellang/Scrutor) automaticamente registrando os serviços no ASP.NET Core DI 
- Testes Unitários, Integrações com **xUnit**, **FluentAssertions**, **Moq**
- Monitoramento de performance da aplicação: [MiniProfiler for .NET](https://miniprofiler.com/dotnet/)
