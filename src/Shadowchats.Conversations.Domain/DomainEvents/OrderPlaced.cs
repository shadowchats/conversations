namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record OrderPlaced : IDomainEvent
{
    public OrderPlaced(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}