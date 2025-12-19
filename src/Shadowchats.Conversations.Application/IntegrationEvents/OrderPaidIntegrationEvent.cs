using MediatR;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed record OrderPaidIntegrationEvent : INotification
{
    private OrderPaidIntegrationEvent()
    {
        OrderId = Guid.Empty;
        PaymentId = Guid.Empty;
    }

    public OrderPaidIntegrationEvent(Guid orderId, Guid paymentId)
    {
        OrderId = orderId;
        PaymentId = paymentId;
    }

    public Guid OrderId { get; private set; }

    public Guid PaymentId { get; private set; }
}