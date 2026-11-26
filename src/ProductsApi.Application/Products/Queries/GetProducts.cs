using ProductsApi.Application.Abstractions.Data;
using ProductsApi.Application.Abstractions.Messaging;

namespace ProductsApi.Application.Products.Queries;

public sealed record GetProductsQuery(string? Search) : IQuery<IReadOnlyList<ProductResponse>>;

internal sealed class GetProductsQueryHandler(IProductsReadStore readStore)
    : IQueryHandler<GetProductsQuery, IReadOnlyList<ProductResponse>>
{
    public Task<IReadOnlyList<ProductResponse>> Handle(GetProductsQuery query, CancellationToken ct)
        => readStore.ListAsync(query.Search, ct);
}
