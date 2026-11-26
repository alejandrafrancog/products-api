using ProductsApi.Application.Abstractions.Data;
using ProductsApi.Application.Abstractions.Messaging;
using ProductsApi.Application.Common;

namespace ProductsApi.Application.Products.Queries;

public sealed record GetProductByIdQuery(int Id) : IQuery<ProductResponse>;

/// <summary>Lado de lectura: va directo al read store, no carga el agregado.</summary>
internal sealed class GetProductByIdQueryHandler(IProductsReadStore readStore)
    : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductByIdQuery query, CancellationToken ct)
        => await readStore.GetByIdAsync(query.Id, ct) ?? throw NotFoundException.Product(query.Id);
}
