using MediatR;
using Shadowchats.Conversations.Domain.DomainEvents;

namespace Shadowchats.Conversations.Application.DomainEventHandlers;

public sealed class OrderPlacedHandler : INotificationHandler<OrderPlacedDomainEvent>
{
    public Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Здесь можно добавить логику:
        // - отправку уведомления
        // - резервирование товаров
        // - создание задач для склада
        
        return Task.CompletedTask;
    }
}