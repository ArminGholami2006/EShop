using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required");
    }
}

internal class DeleteProductCommandHandler(ApplicationContext context) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new ProductNotFoundException(command.Id);

        context.Remove(product);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
