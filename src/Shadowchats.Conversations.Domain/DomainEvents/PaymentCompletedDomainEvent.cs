using MediatR;

namespace Shadowchats.Conversations.Domain.DomainEvents;

public sealed record PaymentCompletedDomainEvent : IRequest<Unit>
{
    public PaymentCompletedDomainEvent(Guid paymentId, Guid orderId)
    {
        PaymentId = paymentId;
        OrderId = orderId;
    }

    public Guid PaymentId { get; }
    
    public Guid OrderId { get; }
}