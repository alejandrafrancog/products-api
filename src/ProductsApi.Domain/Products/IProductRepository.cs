namespace BootcampCLTNoviembre.Domain.Products;

/// <summary>
/// Repositorio del LADO DE ESCRITURA: solo carga y guarda el agregado para aplicar reglas.
/// Las lecturas van por un modelo de lectura separado (IProductsReadStore en Application).
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Indica si ya existe otro producto con ese nombre (sin distinguir mayusculas).</summary>
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken ct = default);

    void Add(Product product);
    void Remove(Product product);
}

/// <summary>Confirma los cambios del comando como una unidad (una transaccion en PostgreSQL).</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}