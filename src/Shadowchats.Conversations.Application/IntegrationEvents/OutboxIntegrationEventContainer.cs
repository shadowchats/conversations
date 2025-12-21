using System.Diagnostics;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Application.Interfaces;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed class OutboxIntegrationEventContainer : BaseIntegrationEventContainer
{
    private OutboxIntegrationEventContainer() : base(Guid.Empty, DateTime.MinValue, null!, null!, null!)
    {
        Status = 0;
    }

    private OutboxIntegrationEventContainer(Guid id, DateTime createdAt, string traceparent, string eventType,
        IIntegrationEvent @event,
        OutboxIntegrationEventStatus status) : base(id, createdAt, traceparent, eventType, @event)
    {
        Status = status;
    }

    public static OutboxIntegrationEventContainer Create(IGuidGenerator guidGenerator,
        IDateTimeProvider dateTimeProvider, ITraceparentProvider traceparentProvider, IIntegrationEvent @event) => new(
        guidGenerator.Generate(), dateTimeProvider.UtcNow, traceparentProvider.Traceparent, @event.EventType, @event,
        OutboxIntegrationEventStatus.Pending);

    public void MarkAsPublished()
    {
        if (Status != OutboxIntegrationEventStatus.Pending)
            throw new BugException();

        Status = OutboxIntegrationEventStatus.Published;
    }

    public OutboxIntegrationEventStatus Status { get; private set; }
}