using ProductsApi.Application.Abstractions.Messaging;
using ProductsApi.Domain.Products;
using FluentValidation;

namespace ProductsApi.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string? Description, decimal Price, int Stock)
    : ICommand<ProductResponse>;

/// <summary>Validacion de formato (400). Se ejecuta automaticamente en el ValidationBehavior.</summary>
internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).ValidName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Price).ValidPrice();
        RuleFor(x => x.Stock).ValidStock();
    }
}

internal sealed class CreateProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken ct)
    {
        await repository.EnsureUniqueNameAsync(command.Name, excludeId: null, ct);

        var product = Product.Create(
            command.Name, command.Description, command.Price, command.Stock,
            timeProvider.GetUtcNow().UtcDateTime);

        repository.Add(product);
        await unitOfWork.SaveChangesAsync(ct); // INSERT ... Retorna el id (IDENTITY de PostgreSQL)

        return product.ToResponse();
    }
}
