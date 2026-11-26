# Etapa 1: restaura dependencias y publica la API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

COPY ["src/ProductsApi.Domain/ProductsApi.Domain.csproj", "src/ProductsApi.Domain/"]
COPY ["src/ProductsApi.Application/ProductsApi.Application.csproj", "src/ProductsApi.Application/"]
COPY ["src/ProductsApi.Infrastructure/ProductsApi.Infrastructure.csproj", "src/ProductsApi.Infrastructure/"]
COPY ["src/ProductsApi.Api/ProductsApi.csproj", "src/ProductsApi.Api/"]

RUN dotnet restore "src/ProductsApi.Api/ProductsApi.csproj" -p:Configuration=Release

COPY . .

RUN dotnet publish "src/ProductsApi.Api/ProductsApi.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Etapa 2: imagen final liviana con runtime de ASP.NET Core.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "ProductsApi.dll"]