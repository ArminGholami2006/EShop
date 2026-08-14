using BuildingBlocks.CQRS;
using Catalog.API.Infrastructure.Persistence;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryQueryHandler(ApplicationContext context) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await context.Products.Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
