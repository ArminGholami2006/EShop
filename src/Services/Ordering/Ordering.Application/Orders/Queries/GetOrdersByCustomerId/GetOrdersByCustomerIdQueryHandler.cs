using BuildingBlocks.CQRS;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomerId;

internal class GetOrdersByCustomerIdQueryHandler(IApplicationContext context) : IQueryHandler<GetOrdersByCustomerIdQuery, GetOrdersByCustomerIdResult>
{
    public async Task<GetOrdersByCustomerIdResult> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrdersByCustomerIdResult(orders.ToOrderDtoList());
    }
}
