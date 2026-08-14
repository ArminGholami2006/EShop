using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomerId;

public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerIdResult>;
public record GetOrdersByCustomerIdResult(IEnumerable<OrderDto> Orders);
