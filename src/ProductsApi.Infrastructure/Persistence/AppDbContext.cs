using ProductsApi.Domain;
using ProductsApi.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ProductsApi.Infrastructure.Persistence;

/// <summary>
/// 1. El Nucleo (DbContext) + 2. Colecciones (DbSet). Tambien es la Unidad de Trabajo:
/// SaveChangesAsync confirma todos los cambios del comando en una sola transaccion.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products => Set<Product>();

    // 3. Configuracion (Fluent API): cada IEntityTypeConfiguration del ensamblado.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
                                           {
                                               SqlState: PostgresErrorCodes.UniqueViolation,
                                               ConstraintName: "ux_products_name_lower"
                                           })
        {
            // Carrera entre dos requests concurrentes que pasaron la validacion previa: el indice unico manda.
            throw new DomainException("Ya existe un producto con ese nombre.");
        }
    }
}
