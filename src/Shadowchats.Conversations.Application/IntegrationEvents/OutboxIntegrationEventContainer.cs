using System.Diagnostics;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed class OutboxIntegrationEventContainer : BaseIntegrationEventContainer
{
    private OutboxIntegrationEventContainer() : base(Guid.Empty, null!, null!, null!)
    {
        Status = OutboxIntegrationEventStatus.None;
    }

    private OutboxIntegrationEventContainer(Guid id, string traceparent, string eventType, IIntegrationEvent @event,
        OutboxIntegrationEventStatus status) : base(id, traceparent, eventType, @event)
    {
        Status = status;
    }

    public static OutboxIntegrationEventContainer Create(IGuidGenerator guidGenerator, IIntegrationEvent @event) => new(guidGenerator.Generate(), Activity.Current?.Id ?? throw new BugException(), @event.EventType, @event, OutboxIntegrationEventStatus.Pending);

    public OutboxIntegrationEventStatus Status { get; private set; }
}