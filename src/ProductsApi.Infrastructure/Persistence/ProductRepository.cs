using ProductsApi.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ProductsApi.Infrastructure.Persistence;

/// <summary>Lado de escritura: entidades con tracking para que EF detecte los cambios del agregado.</summary>
internal sealed class ProductRepository(AppDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    // Se traduce a: lower(name) = lower(@name)
    public Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default)
        => db.Products.AnyAsync(
            p => p.Name.ToLower() == name.ToLower() && (excludeId == null || p.Id != excludeId), ct);

    public void Add(Product product) => db.Products.Add(product);

    public void Remove(Product product) => db.Products.Remove(product);
}
