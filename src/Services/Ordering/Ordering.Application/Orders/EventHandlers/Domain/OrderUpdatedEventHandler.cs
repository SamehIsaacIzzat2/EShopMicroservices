namespace Ordering.Application.Orders.EventHandlers.Domain
{
    internal class OrderUpdatedEventHandler : INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
