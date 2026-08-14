using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

internal class DeleteOrderCommandHandler(IApplicationContext context) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await context.Orders.FindAsync([orderId], cancellationToken) ?? throw new OrderNotFoundException(command.OrderId);

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
