using Shadowchats.Conversations.Application.Enums;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public class OutboxIntegrationEvent : BaseIntegrationEvent
{
    private OutboxIntegrationEvent() : base(Guid.Empty, string.Empty, string.Empty, null!)
    {
        Status = default;
    }

    private OutboxIntegrationEvent(Guid id, string traceparent, string eventName, object payload,
        OutboxIntegrationEventStatus status) : base(id, traceparent, eventName, payload)
    {
        Status = status;
    }

    public OutboxIntegrationEventStatus Status { get; private set; }
}