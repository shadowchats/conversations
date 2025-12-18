namespace Shadowchats.Conversations.Application.IntegrationEvents;

public class OrderPaidOutboxIntegrationEvent : OutboxIntegrationEvent
{
    private OrderPaidOutboxIntegrationEvent(Guid id, string traceparent, string eventName, object payload,
        OutboxIntegrationEventStatus status) : base(id, traceparent, eventName, payload)
    {
        Status = status;
    }

    public static OutboxIntegrationEvent<TPayload>
        Create(IGuidGenerator guidGenerator, string traceparent, TPayload payload) => new(guidGenerator.Generate(),
        traceparent, TPayload.EventName, payload, OutboxIntegrationEventStatus.Pending);
}

public sealed record OrderPaidOutboxIntegrationEventPayload
{
    private OrderPaidOutboxIntegrationEventPayload()
    {
        OrderId = Guid.Empty;
        PaymentId = Guid.Empty;
    }

    public OrderPaidOutboxIntegrationEventPayload(Guid orderId, Guid paymentId)
    {
        OrderId = orderId;
        PaymentId = paymentId;
    }

    public Guid OrderId { get; private set; }

    public Guid PaymentId { get; private set; }

    public static string EventName { get; } = "OrderPaidOutboxIntegrationEvent";
}