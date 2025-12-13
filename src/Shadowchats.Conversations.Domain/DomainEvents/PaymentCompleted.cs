using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record PaymentCompleted : INotification
{
    public PaymentCompleted(Guid paymentId, Guid orderId)
    {
        PaymentId = paymentId;
        OrderId = orderId;
    }

    public Guid PaymentId { get; }
    
    public Guid OrderId { get; }
}