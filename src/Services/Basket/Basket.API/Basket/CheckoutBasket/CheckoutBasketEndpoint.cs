using Basket.API.Basket.DeleteBasket;
using Basket.API.Dtos;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BaseCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CheckoutBasketCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        })
            .WithName("CheckoutBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket")
            .WithDescription("Checkout Basket");
    }
}
