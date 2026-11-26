using ProductsApi.Application.Abstractions.Messaging;
using ProductsApi.Application.Common;
using ProductsApi.Domain.Products;

namespace ProductsApi.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : ICommand<Unit>;

internal sealed class DeleteProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(command.Id, ct) ?? throw NotFoundException.Product(command.Id);

        repository.Remove(product);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
