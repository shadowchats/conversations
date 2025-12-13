using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPlaced : INotification
{
    public OrderPlaced(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}