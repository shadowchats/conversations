using MediatR;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public abstract class BaseIntegrationEventContainer
{
    protected BaseIntegrationEventContainer(Guid id, string traceparent, string eventType, INotification @event)
    {
        Id = id;
        Traceparent = traceparent;
        EventType = eventType;
        Event = @event;
    }
    
    public Guid Id { get; private set; }
    
    public string Traceparent { get; private set; }
    
    public string EventType { get; private set; }
    
    public INotification Event { get; private set; }
}