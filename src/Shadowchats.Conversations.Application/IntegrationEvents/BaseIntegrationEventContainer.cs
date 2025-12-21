namespace Shadowchats.Conversations.Application.IntegrationEvents;

public abstract class BaseIntegrationEventContainer
{
    protected BaseIntegrationEventContainer(Guid id, DateTime createdAt, string traceparent, string eventType, IIntegrationEvent @event)
    {
        Id = id;
        CreatedAt = createdAt;
        Traceparent = traceparent;
        EventType = eventType;
        Event = @event;
    }
    
    public Guid Id { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public string Traceparent { get; private set; }
    
    public string EventType { get; private set; }
    
    public IIntegrationEvent Event { get; private set; }
}