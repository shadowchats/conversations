using MediatR;

namespace Shadowchats.Conversations.Application.IntegrationEvents;

public abstract class BaseIntegrationEvent : INotification
{
    protected BaseIntegrationEvent(Guid id, string traceparent, string eventName, object payload)
    {
        Id = id;
        Traceparent = traceparent;
        EventName = eventName;
        Payload = payload;
    }
    
    public Guid Id { get; private set; }
    
    public string Traceparent { get; private set; }
    
    public string EventName { get; private set; }
    
    public object Payload { get; private set; }
}