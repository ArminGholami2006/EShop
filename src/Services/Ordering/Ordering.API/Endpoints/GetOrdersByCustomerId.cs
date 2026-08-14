using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrdersByCustomerId;

namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerIdResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomerId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersByCustomerIdQuery(customerId));

            var response = result.Adapt<GetOrdersByCustomerIdResponse>();

            return Results.Ok(response);
        })
            .WithName("GetOrdersByCustomerId")
            .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders By Customer Id")
            .WithDescription("Get Orders By Customer Id");
    }
}
