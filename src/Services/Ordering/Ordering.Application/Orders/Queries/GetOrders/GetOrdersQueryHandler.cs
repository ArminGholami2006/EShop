using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;

internal class GetOrdersQueryHandler(IApplicationContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var type = context.Orders.GetType();
        var interfaces = type.GetInterfaces();
        var result = interfaces.Select(x => x.FullName);
        foreach (var r in result)
        {
            Console.WriteLine("*************************************************************+");
            Console.WriteLine(r);
        }

        var totalCount = await context.Orders.LongCountAsync(cancellationToken);

        var orders = await context.Orders
            .Include(x => x.OrderItems)
            .OrderBy(x => x.OrderName.Value)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orders.ToOrderDtoList()));
    }
}
