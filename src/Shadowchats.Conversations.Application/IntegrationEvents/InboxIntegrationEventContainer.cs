using System.Diagnostics;
using Shadowchats.Conversations.Application.Enums;
using Shadowchats.Conversations.Domain.Exceptions;
using Shadowchats.Conversations.Domain.Interfaces;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed class InboxIntegrationEventContainer : BaseIntegrationEventContainer
{
    private InboxIntegrationEventContainer() : base(Guid.Empty, null!, null!, null!)
    {
        Status = 0;
    }

    private InboxIntegrationEventContainer(Guid id, string traceparent, string eventType, IIntegrationEvent @event,
        InboxIntegrationEventStatus status) : base(id, traceparent, eventType, @event)
    {
        Status = status;
    }

    public static InboxIntegrationEventContainer Create(IGuidGenerator guidGenerator, IIntegrationEvent @event) => new(
        guidGenerator.Generate(), Activity.Current?.Id ?? throw new BugException(), @event.EventType, @event,
        InboxIntegrationEventStatus.Pending);

    public InboxIntegrationEventStatus Status { get; private set; }
}