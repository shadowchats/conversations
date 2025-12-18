using Shadowchats.Conversations.Application.IntegrationEvents;

namespace Shadowchats.Conversations.Application.Interfaces;

public interface IOutboxIntegrationEventRepository<TPayload> where TPayload : IIntegrationEventPayload<TPayload>
{
    Task Add(OutboxIntegrationEvent<TPayload> integrationEvent, CancellationToken cancellationToken);
}