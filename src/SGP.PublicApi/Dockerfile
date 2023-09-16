# https://hub.docker.com/_/microsoft-dotnet-sdk/ 
ARG sdk_version=7.0-bookworm-slim 

FROM mcr.microsoft.com/dotnet/aspnet:$sdk_version AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_NOLOGO=1
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_EnableDiagnostics=0

FROM mcr.microsoft.com/dotnet/sdk:$sdk_version AS build
ARG configuration=Release
WORKDIR /src
COPY ["Directory.Build.props", "src/"]
COPY ["Directory.Packages.props", "src/"]
COPY ["src/SGP.PublicApi/SGP.PublicApi.csproj", "src/SGP.PublicApi/"]
COPY ["src/SGP.Application/SGP.Application.csproj", "src/SGP.Application/"]
COPY ["src/SGP.Shared/SGP.Shared.csproj", "src/SGP.Shared/"]
COPY ["src/SGP.Domain/SGP.Domain.csproj", "src/SGP.Domain/"]
COPY ["src/SGP.Infrastructure/SGP.Infrastructure.csproj", "src/SGP.Infrastructure/"]
RUN dotnet restore "src/SGP.PublicApi/SGP.PublicApi.csproj"
COPY . .
WORKDIR "/src/src/SGP.PublicApi"
RUN dotnet build "SGP.PublicApi.csproj" --configuration $configuration --no-restore --nologo --output /app/build 

FROM build AS publish
RUN dotnet publish "SGP.PublicApi.csproj" --configuration $configuration --no-restore --nologo --self-contained true --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGP.PublicApi.dll"]
