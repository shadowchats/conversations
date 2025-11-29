namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPaid : IDomainEvent
{
    public OrderPaid(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}