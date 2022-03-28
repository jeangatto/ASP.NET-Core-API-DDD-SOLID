#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SGP.PublicApi/SGP.PublicApi.csproj", "src/SGP.PublicApi/"]
COPY ["src/SGP.Application/SGP.Application.csproj", "src/SGP.Application/"]
COPY ["src/SGP.Shared/SGP.Shared.csproj", "src/SGP.Shared/"]
COPY ["src/SGP.Domain/SGP.Domain.csproj", "src/SGP.Domain/"]
COPY ["src/SGP.Infrastructure/SGP.Infrastructure.csproj", "src/SGP.Infrastructure/"]
RUN dotnet restore "src/SGP.PublicApi/SGP.PublicApi.csproj"
COPY . .
WORKDIR "/src/src/SGP.PublicApi"
RUN dotnet build "SGP.PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SGP.PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGP.PublicApi.dll"]
