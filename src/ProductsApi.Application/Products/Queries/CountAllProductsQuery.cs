using ProductsApi.Application.Abstractions.Data;
using ProductsApi.Application.Abstractions.Messaging;

namespace ProductsApi.Application.Products.Queries;

public sealed record CountAllProductsQuery() : IQuery<ProductCountResponse>;

internal sealed class CountAllProductsQueryHandler(IProductsReadStore readStore)
    : IQueryHandler<CountAllProductsQuery, ProductCountResponse>
{
    public async Task<ProductCountResponse> Handle(CountAllProductsQuery query, CancellationToken ct)
        => new(await readStore.CountAsync(ct));
}