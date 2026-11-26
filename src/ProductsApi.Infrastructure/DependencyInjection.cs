using ProductsApi.Application.Abstractions.Data;
using ProductsApi.Domain.Products;
using ProductsApi.Infrastructure.Persistence;
using ProductsApi.Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsApi.Infrastructure;

public static class DependencyInjection
{
    public const string ConnectionStringName = "BootcampDb";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                $"Falta la cadena de conexion 'ConnectionStrings:{ConnectionStringName}'. " +
                "Configurarla en appsettings.Development.json, user-secrets o la variable de entorno " +
                $"ConnectionStrings__{ConnectionStringName}.");

        // Npgsql gestiona el Connection Pooling automaticamente (Pooling=true por defecto).
        // EnableRetryOnFailure reintenta ante fallas transitorias de red/servidor.
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
            connectionString,
            npgsql => npgsql.EnableRetryOnFailure(maxRetryCount: 3)));

        // Escritura
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();

        // Lectura (podria usar otra conexion, ej. una replica de solo lectura)
        services.AddScoped<IProductsReadStore, ProductsReadStore>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
