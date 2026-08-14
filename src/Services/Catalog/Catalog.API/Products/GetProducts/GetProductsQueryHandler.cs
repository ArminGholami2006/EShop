using BuildingBlocks.CQRS;
using Catalog.API.Infrastructure.Persistence;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(ApplicationContext context) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await context.Products.AsNoTracking().Skip(query.PageNumber).Take(query.PageSize).ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}
