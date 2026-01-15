FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Update package lists
RUN apt-get update && apt-get install -y --no-install-recommends \
    openssl \
    libssl3 \
    # Clean up apt cache
    && apt-get clean \
    # Remove apt lists to reduce image size
    && rm -rf /var/lib/apt/lists/* \
    # Create a non-root user and group
    && groupadd -r appuser && useradd -r -g appuser appuser

# Switch to non-root user 
USER appuser

WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

# Specifies whether .NET welcome and telemetry messages are displayed on the first run.
ENV DOTNET_NOLOGO=1

# Specifies whether data about the .NET tools usage is collected and sent to Microsoft.
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
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
RUN dotnet publish "SGP.PublicApi.csproj" --configuration Release --no-restore --nologo --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGP.PublicApi.dll"]
