namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed record OrderPaidIntegrationEvent : IIntegrationEvent
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
    
    string IIntegrationEvent.EventType => EventType;
    
    public const string EventType = "OrderPaid";
}