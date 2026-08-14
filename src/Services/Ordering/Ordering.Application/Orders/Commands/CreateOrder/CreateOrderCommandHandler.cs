using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;

internal class CreateOrderCommandHandler(IApplicationContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private static Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddressDto = orderDto.ShippingAddress;
        var billingAddressDto = orderDto.BillingAddress;
        var paymentDto = orderDto.Payment;

        var shippingAddress = Address.Of(shippingAddressDto.FirstName, shippingAddressDto.LastName, shippingAddressDto.EmailAddress, shippingAddressDto.AddressLine, shippingAddressDto.Country, shippingAddressDto.State, shippingAddressDto.ZipCode);
        var billingAddress = Address.Of(billingAddressDto.FirstName, billingAddressDto.LastName, billingAddressDto.EmailAddress, billingAddressDto.AddressLine, billingAddressDto.Country, billingAddressDto.State, billingAddressDto.ZipCode);
        var payment = Payment.Of(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.CVV, paymentDto.PaymentMethod);

        var newOrder = Order.Create(
            OrderId.Of(Guid.NewGuid()),
            CustomerId.Of(Guid.NewGuid()),
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            payment);

        return newOrder;
    }
}
