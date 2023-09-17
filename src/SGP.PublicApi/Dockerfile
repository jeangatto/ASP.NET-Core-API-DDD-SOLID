FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim  AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_NOLOGO=1
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim  AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/SGP.PublicApi/SGP.PublicApi.csproj", "src/SGP.PublicApi/"]
COPY ["src/SGP.Application/SGP.Application.csproj", "src/SGP.Application/"]
COPY ["src/SGP.Shared/SGP.Shared.csproj", "src/SGP.Shared/"]
COPY ["src/SGP.Domain/SGP.Domain.csproj", "src/SGP.Domain/"]
COPY ["src/SGP.Infrastructure/SGP.Infrastructure.csproj", "src/SGP.Infrastructure/"]
RUN dotnet restore "src/SGP.PublicApi/SGP.PublicApi.csproj"
COPY . .
WORKDIR "/src/src/SGP.PublicApi"
RUN dotnet build "SGP.PublicApi.csproj" --configuration Release --no-restore --nologo --output /app/build 

FROM build AS publish
RUN dotnet publish "SGP.PublicApi.csproj" --configuration Release --no-restore --nologo --self-contained true --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGP.PublicApi.dll"]
