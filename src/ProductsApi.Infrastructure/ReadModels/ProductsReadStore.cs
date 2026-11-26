using ProductsApi.Application.Abstractions.Data;
using ProductsApi.Application.Products;
using ProductsApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ProductsApi.Infrastructure.ReadModels;

/// <summary>
/// Lado de LECTURA: proyecciones directas a DTO, sin tracking y sin pasar por el agregado.
/// EF genera un SELECT solo con las columnas necesarias. En produccion podria apuntar a una
/// replica de lectura de PostgreSQL o usar Dapper con SQL propio.
/// </summary>
internal sealed class ProductsReadStore(AppDbContext db) : IProductsReadStore
{
    public Task<ProductResponse?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt))
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<ProductResponse>> ListAsync(string? search, CancellationToken ct = default)
    {
        var query = db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term)); // parametrizado: sin inyeccion SQL
        }

        return await query
            .OrderBy(p => p.Id)
            .Select(p => new ProductResponse(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt))
            .ToListAsync(ct);
    }
}
