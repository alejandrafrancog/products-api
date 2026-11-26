
namespace BootcampCLTNoviembre.Domain.Products;

/// <summary>
/// Agregado raiz. Modelo rico: el estado solo cambia a traves de metodos que protegen las invariantes.
/// No conoce EF Core, PostgreSQL ni HTTP: el mapeo a la tabla "products" vive en Infrastructure.
/// </summary>
public sealed class Product
{
    public const int NameMaxLength = 150;
    public const int DescriptionMaxLength = 500;

    private Product() { } // Requerido por el ORM

    /// <summary>Lo genera PostgreSQL (columna IDENTITY) al insertar; EF Core lo asigna tras SaveChanges.</summary>
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Product Create(string name, string? description, decimal price, int stock, DateTime now)
    {
        var product = new Product { CreatedAt = now };
        product.SetData(name, description, price, stock);
        return product;
    }

    public void Update(string name, string? description, decimal price, int stock, DateTime now)
    {
        SetData(name, description, price, stock);
        UpdatedAt = now;
    }

    private void SetData(string name, string? description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre del producto es obligatorio.");
        if (price < 0)
            throw new DomainException("El precio no puede ser negativo.");
        if (stock < 0)
            throw new DomainException("El stock no puede ser negativo.");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Price = price;
        Stock = stock;
    }
}