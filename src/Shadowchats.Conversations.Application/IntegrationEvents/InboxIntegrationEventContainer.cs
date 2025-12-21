using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed class InboxIntegrationEventContainer : BaseIntegrationEventContainer
{
    private InboxIntegrationEventContainer() : base(Guid.Empty, DateTime.MinValue, null!, null!, null!)
    {
        Status = 0;
    }

    private InboxIntegrationEventContainer(Guid id, DateTime createdAt, string traceparent, string eventType, IIntegrationEvent @event,
        InboxIntegrationEventStatus status) : base(id, createdAt, traceparent, eventType, @event)
    {
        Status = status;
    }

    public static InboxIntegrationEventContainer Create(IGuidGenerator guidGenerator, IDateTimeProvider dateTimeProvider, ITraceparentProvider traceparentProvider, IIntegrationEvent @event) => new(
        guidGenerator.Generate(), dateTimeProvider.UtcNow, traceparentProvider.Traceparent, @event.EventType, @event,
        InboxIntegrationEventStatus.Pending);

    public InboxIntegrationEventStatus Status { get; private set; }
}