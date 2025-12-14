using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPaidDomainEvent : INotification
{
    public OrderPaidDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}