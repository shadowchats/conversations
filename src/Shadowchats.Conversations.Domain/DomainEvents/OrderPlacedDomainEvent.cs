using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPlacedDomainEvent : INotification
{
    public OrderPlacedDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}