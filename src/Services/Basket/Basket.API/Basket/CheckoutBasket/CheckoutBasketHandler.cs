using Basket.API.Dtos;
using Basket.API.Infrastructure.Persistence.Data;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BaseCheckoutDto) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BaseCheckoutDto)
            .NotNull().WithMessage("BasketCheckoutDto cannot be null");
        RuleFor(x => x.BaseCheckoutDto.Username)
            .NotEmpty().WithMessage("Username is required");
    }
}

internal class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.BaseCheckoutDto.Username, cancellationToken);
        if (basket is null)
        {
            return new CheckoutBasketResult(false);
        }

        var eventMessage = command.BaseCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasket(command.BaseCheckoutDto.Username, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
