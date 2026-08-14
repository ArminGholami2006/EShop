using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler(ApplicationContext context) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        context.Update(product);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
