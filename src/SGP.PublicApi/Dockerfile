# https://hub.docker.com/_/microsoft-dotnet-aspnet

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64 AS base
WORKDIR /app
EXPOSE 80

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim-amd64 AS build
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
