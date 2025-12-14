using MediatR;
using Shadowchats.Conversations.Domain.DomainEvents;

namespace Shadowchats.Conversations.Application.DomainEventHandlers;

public sealed class OrderPaidHandler : INotificationHandler<OrderPaidDomainEvent>
{
    public Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        // Здесь можно добавить логику:
        // - отправку уведомления клиенту
        // - активацию процесса доставки
        // - обновление статуса в системе учёта
        
        return Task.CompletedTask;
    }
}