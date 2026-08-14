using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}", async (string username, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(username));

            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
    }
}
