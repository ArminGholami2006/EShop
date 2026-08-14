using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Infrastructure.Persistence;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(ApplicationContext context) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        return product is null
            ? throw new ProductNotFoundException(query.Id)
            : new GetProductByIdResult(product);
    }
}
