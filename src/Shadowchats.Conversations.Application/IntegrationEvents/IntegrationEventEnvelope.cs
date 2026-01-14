namespace Shadowchats.Conversations.Application.IntegrationEvents;

public sealed record IntegrationEventEnvelope
{
    public required string EventType { get; init; }
    
    public required string Payload { get; init; }
    
    public required string Traceparent { get; init; }
}