using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPaid : INotification
{
    public OrderPaid(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}