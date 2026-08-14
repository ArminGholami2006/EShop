using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

internal class UpdateOrderCommandHandler(IApplicationContext context) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await context.Orders.FindAsync([orderId], cancellationToken) ?? throw new OrderNotFoundException(command.Order.Id);
        UpdateOrderWithNewValue(order, command.Order);

        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrderWithNewValue(Order order, OrderDto orderDto)
    {
        var shippingAddress = orderDto.ShippingAddress;
        var billingAddress = orderDto.BillingAddress;
        var payment = orderDto.Payment;

        var newShippingAddress = Address.Of(shippingAddress.FirstName, shippingAddress.LastName, shippingAddress.EmailAddress, shippingAddress.AddressLine, shippingAddress.Country, shippingAddress.State, shippingAddress.ZipCode);
        var newBillingAddress = Address.Of(billingAddress.FirstName, billingAddress.LastName, billingAddress.EmailAddress, billingAddress.AddressLine, billingAddress.Country, billingAddress.State, billingAddress.ZipCode);
        var newPayment = Payment.Of(payment.CardName, payment.CardNumber, payment.Expiration, payment.CVV, payment.PaymentMethod);

        order.Update(
            OrderName.Of(orderDto.OrderName),
            newShippingAddress,
            newBillingAddress,
            newPayment,
            orderDto.Status);
    }
}
